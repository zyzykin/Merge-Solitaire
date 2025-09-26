using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class EndGameController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private ScoreBox scoreBox;
        [Header("Buttons")]
        [SerializeField] private Button menuButton;
        [SerializeField] private Button playButton;

        private void OnEnable()
        {
            var won = PlayerPrefs.GetInt(Consts.PREFS_LAST_WON, 0) == 1;
            var lastScore = PlayerPrefs.GetInt(Consts.PREFS_LAST_SCORE, 0);
            Debug.Log("Last score: " + lastScore);
            scoreBox.SetScore(lastScore);
            
            menuButton.onClick.AddListener(OnMenuButtonClicked);
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnDisable()
        {
            menuButton.onClick.RemoveListener(OnMenuButtonClicked);
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }

        private void OnMenuButtonClicked()
        {
            SceneManager.LoadScene(Consts.SCENE_MAIN_MENU);
        }

        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene(Consts.SCENE_GAME);
        }
    }
}