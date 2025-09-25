using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Whatwapp.MergeSolitaire.Game.Presentation;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class VictoryState : BaseState
    {
        private readonly ISFXPresenter _sfxPresenter;

        private const float ShowPanelDelay1 = 1f;
        private const float ShowPanelDelay2 = 2f;
        
        public VictoryState(GameController gameController, SFXPresenter sfxPresenter) : base(gameController)
        {
            _sfxPresenter = sfxPresenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _gameController.Score += Consts.VICTORY_POINTS;
            PlayerPrefs.SetInt(Consts.PREFS_LAST_WON, 1);

            _gameController.StartCoroutine(ShowPanel());
        }

        private IEnumerator ShowPanel()
        {
            yield return new WaitForSeconds(ShowPanelDelay1);
            _sfxPresenter.PlayOneShot(Consts.SFX_Victory);
            yield return new WaitForSeconds(ShowPanelDelay2);
            SceneManager.LoadScene(Consts.SCENE_END_GAME);
        }
    }
}