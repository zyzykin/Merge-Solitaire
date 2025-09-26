using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class ScoreBox : MonoBehaviour
    {
        [SerializeField] private float changeScoreDuration = 1f;
        [Header("References")] [SerializeField]
        private TMPro.TMP_Text scoreText;

        private int _currentScore;
        private Tween _changeScoreTween;

        public void SetScore(int score, bool animate = true)
        {
            if(_changeScoreTween != null)
            {
                _changeScoreTween.Kill();
            }
            
            if ((_currentScore >= score) || !animate)
            {
                SetImmediate(score);
                return;
            }
            
            _changeScoreTween = DOVirtual.Int(_currentScore, score, changeScoreDuration, (value) =>
            {
                scoreText.text = value.ToString();
                _currentScore = value;
            });
        }

        private void SetImmediate(int score)
        {
            scoreText.text = score.ToString();
            _currentScore = score;
        }
    }
}