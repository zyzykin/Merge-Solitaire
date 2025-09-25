using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private ScoreBox scoreBox;

        private void OnEnable()
        {
            var lastScore = PlayerPrefs.GetInt(Consts.PREFS_LAST_SCORE, 0);
            scoreBox.SetScore(lastScore);
        }
    }
}