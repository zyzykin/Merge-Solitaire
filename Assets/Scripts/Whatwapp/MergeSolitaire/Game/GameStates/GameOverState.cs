using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Whatwapp.MergeSolitaire.Game.Presentation;
using Whatwapp.MergeSolitaire.Game.Scenes;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class GameOverState : BaseState
    {
        private readonly ISFXPresenter _sfxPresenter;

        public GameOverState(GameController gameController, ISFXPresenter sfxPresenter) : base(gameController)
        {
            _sfxPresenter = sfxPresenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            PlayerPrefs.SetInt(Consts.PREFS_LAST_WON, 0);
            GameController.StartCoroutine(ShowPanel());
        }

        private IEnumerator ShowPanel()
        {
            yield return new WaitForSeconds(1f);
            _sfxPresenter.PlayOneShot(Consts.SFX_Lost);
            yield return new WaitForSeconds(2f);

            SceneLoadingManager.Instance.LoadEndGame();
           // SceneManager.LoadScene(Consts.SCENE_END_GAME);
        }
    }
}