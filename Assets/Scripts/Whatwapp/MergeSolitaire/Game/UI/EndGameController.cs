using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class EndGameController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private ScoreBox scoreBox;

        private void Start()
        {
            var won = PlayerPrefs.GetInt(Consts.PREFS_LAST_WON, 0) == 1;
            var lastScore = PlayerPrefs.GetInt(Consts.PREFS_LAST_SCORE, 0);
            Debug.Log("Last score: " + lastScore);
            scoreBox.SetScore(lastScore);
        }
    }
}