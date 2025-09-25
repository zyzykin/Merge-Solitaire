using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private ScoreBox scoreBox;

        private void Start()
        {
            var highscore = PlayerPrefs.GetInt(Consts.PREFS_HIGHSCORE, 0);
            Debug.Log("Highscore: " + highscore);
            scoreBox.SetScore(highscore);
        }
    }
}