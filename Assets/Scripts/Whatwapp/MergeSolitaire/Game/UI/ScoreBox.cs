using System.Collections;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class ScoreBox : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMPro.TMP_Text _scoreText;
        
        private int _currentScore;
        
        
        
        public void SetScore(int score, bool animate = true)
        {
            if ((score == 0) || (_currentScore>=score) || !animate)
            {
                SetImmediate(score);
                return;
            }
            StartCoroutine(UpdateScore(score));
        }

        private void SetImmediate(int score)
        {
            _scoreText.text = score.ToString();
            _currentScore = score;
        }

        private IEnumerator UpdateScore(int score)
        {
            var currentScore = _currentScore;
            _currentScore = score;
            var delta = score - currentScore;
            var step = Mathf.Max(1, delta / 10);
            while (currentScore < score)
            {
                currentScore += step;
                currentScore = Mathf.Min(currentScore, score);
                _scoreText.text = currentScore.ToString();
                yield return null;
            }
        }
    }
}