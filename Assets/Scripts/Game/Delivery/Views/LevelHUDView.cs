using System;
using System.Collections;
using Shoelace.Bejeweld.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shoelace.Bejeweld.Views
{
    public class LevelHUDView : MonoBehaviour, ILevelHudView
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Slider progressBar;
        [SerializeField] private Vector3 maxScale;
        [SerializeField] private float upTextDuration;
        [SerializeField] private float downTextDuration;
        private Coroutine _scaleUpCoroutine;

        public void UpdateProgress(int score)
        {
            scoreText.text = score.ToString();
            progressBar.value = score;
            ScaleTextAnimation();
        }

        public void SetLevel(Level level)
        {
            levelText.text = "LEVEL " + level.levelId;
            progressBar.maxValue = level.goal;
        }

        private void ScaleTextAnimation()
        {
            if (_scaleUpCoroutine != null) StopCoroutine(_scaleUpCoroutine);
            _scaleUpCoroutine = StartCoroutine(ScaleUpText(upTextDuration, downTextDuration));
        }

        private IEnumerator ScaleUpText(float upDuration, float downDuration)
        {
            float time = 0;
            var initialScale = scoreText.transform.localScale;
            while (time < upDuration)
            {
                time += Time.deltaTime;
                scoreText.transform.localScale = Vector3.Lerp(initialScale, maxScale, time / upDuration);
                yield return null;
            }

            time = 0;
            
            while (time < downDuration)
            {
                time += Time.deltaTime;
                scoreText.transform.localScale = Vector3.Lerp(maxScale, Vector3.one, time / downDuration);
                yield return null;
            }
        }
    }

}

[Serializable]
public class Level
{
    public int goal;
    public int levelId;
}