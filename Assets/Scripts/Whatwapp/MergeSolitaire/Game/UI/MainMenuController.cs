using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Whatwapp.MergeSolitaire.Game.Scenes;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private ScoreBox scoreBox;
        [SerializeField] private Button playButton;

        private void Start()
        {
            var highscore = PlayerPrefs.GetInt(Consts.PREFS_HIGHSCORE, 0);
            Debug.Log("Highscore: " + highscore);
            scoreBox.SetScore(highscore);
        }

        private void OnEnable()
        {
            playButton.onClick.AddListener(OnPlayClicked);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnPlayClicked);
        }

        private void OnPlayClicked()
        {
            SceneLoadingManager.Instance.LoadGame();
           // SceneManager.LoadScene(Consts.SCENE_GAME);
        }
    }
}