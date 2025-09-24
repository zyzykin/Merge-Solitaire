using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScoreBox _scoreBox;
        
        private void OnEnable()
        {
            var lastScore = PlayerPrefs.GetInt(Consts.PREFS_LAST_SCORE, 0);
            _scoreBox.SetScore(lastScore);
            
        }
    }
}