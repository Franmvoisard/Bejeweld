using System;
using System.Collections.Generic;
using System.Linq;
using Shoelace.Bejeweld.Views;
using UnityEngine;

namespace Shoelace.Bejeweld.Components
{
    public class LevelController : MonoBehaviour
    {
        private const string LastPlayedLevelKey = "LAST_PLAYED_LEVEL";
        
        [SerializeField] private List<Level> levels;
        [SerializeField] private LevelHUDView webGLHud;
        [SerializeField] private LevelHUDView mobileHUDView;
        [SerializeField] private Transform hudParent;

        [SerializeField] private int scorePerTile;
        [SerializeField] private WinPopup winPopup;
        
        private ILevelHudView _levelHudView;
        private int _currentScore;
        private Level _currentLevel;

        public static event Action GameCompleted;

        private bool gameCompleted = false;
        private void Awake()
        {
            Application.targetFrameRate = 60;
            InstantiateHudForPlatform();
            _currentLevel = GetCurrentLevel();
            LoadLevel(_currentLevel);
            TileSelector.EnableSelection();
            GridView.OnMatchesCleared += SumPoints;
        }

        private void SumPoints(int matches)
        {
            _currentScore += matches * scorePerTile;
            _levelHudView.UpdateProgress(_currentScore);
            CheckVictory();
        }

        private void CheckVictory()
        {
            if (gameCompleted) return;
            if (_currentScore >= _currentLevel.goal)
            {
                Win();
            }
        }

        private void Win()
        {
            Instantiate(winPopup, hudParent);
            gameCompleted = true;
            TileSelector.Disable();
            GameCompleted?.Invoke();
        }

        public Level GetCurrentLevel()
        {
            var lastPlayedLevel = PlayerPrefs.GetInt(LastPlayedLevelKey, 1);
            var level = levels.FirstOrDefault(level => level.levelId == lastPlayedLevel);

            if (level != null)  return level;
            else
            {
                SaveLastPlayedLevel(1);
                return levels[0];
            }
        }
        
        public void SaveLastPlayedLevel(int levelId)
        {
            PlayerPrefs.SetInt(LastPlayedLevelKey, levelId);
            PlayerPrefs.Save();
        }

        private void InstantiateHudForPlatform()
        {
#if UNITY_WEBGL
            InstantiateWebGLHud();
#elif UNITY_IOS || UNITY_ANDROID
            InstantiateMobileHUD();
#endif
        }

        private void InstantiateWebGLHud()
        {
            _levelHudView = Instantiate(webGLHud, hudParent);
        }

        private void InstantiateMobileHUD()
        {
            _levelHudView = Instantiate(mobileHUDView, hudParent);
        }

        private void OnDestroy()
        {
            GridView.OnMatchesCleared -= SumPoints;

        }

        public void LoadLevel(Level level)
        {
            _levelHudView.SetLevel(level);
        }
    }
}