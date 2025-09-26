using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private ScoreBox scoreBox;

        [Header("Buttons")] [SerializeField] private Button menuButton;
        [SerializeField] private Button resumeButton;

        private void OnEnable()
        {
            var lastScore = PlayerPrefs.GetInt(Consts.PREFS_LAST_SCORE, 0);
            scoreBox.SetScore(lastScore, false);

            menuButton.onClick.AddListener(OnMenuButtonClicked);
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
        }

        private void OnDisable()
        {
            menuButton.onClick.RemoveListener(OnMenuButtonClicked);
            resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
        }

        private void OnMenuButtonClicked()
        {
            SceneManager.LoadScene(Consts.SCENE_MAIN_MENU);
        }

        private void OnResumeButtonClicked()
        {
            var gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                gameController.IsPaused = false;
            }

            SceneManager.UnloadSceneAsync(Consts.SCENE_GAME);
        }
    }
}