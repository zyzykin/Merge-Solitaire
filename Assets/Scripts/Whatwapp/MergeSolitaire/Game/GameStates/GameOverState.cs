using System.Collections;
using UnityEngine;
using Whatwapp.MergeSolitaire.Game.Presentation;
using Whatwapp.MergeSolitaire.Game.Scenes;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class GameOverState : BaseState
    {
        private readonly ISFXPresenter _sfxPresenter;

        private const float ShowPanelDelay1 = 1f;
        private const float ShowPanelDelay2 = 2f;
        
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
            yield return new WaitForSeconds(ShowPanelDelay1);
            _sfxPresenter.PlayOneShot(Consts.SFX_Lost);
            yield return new WaitForSeconds(ShowPanelDelay2);

            SceneLoadingManager.Instance.LoadEndGame();
        }
    }
}