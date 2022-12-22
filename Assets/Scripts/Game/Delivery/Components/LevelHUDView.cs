using System;
using Codice.Client.BaseCommands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Shoelace.Bejeweld.Components
{
    public class LevelHUDView : MonoBehaviour, ILevelHudView
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Slider progressBar;
        
        public void UpdateProgress(int score)
        {
            scoreText.text = score.ToString();
            progressBar.value = score;
        }

        public void SetLevel(Level level)
        {
            levelText.text = "LEVEL " + level.levelId;
            progressBar.maxValue = level.goal;
        }
    }

    public interface ILevelHudView
    {
        public void UpdateProgress(int score);
        void SetLevel(Level level);
    }
}

[Serializable]
public class Level
{
    public int goal;
    public int levelId;
}