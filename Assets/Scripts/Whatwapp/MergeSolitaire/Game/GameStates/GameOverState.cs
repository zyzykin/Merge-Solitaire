using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Whatwapp.Core.Audio;
using Whatwapp.MergeSolitaire.Game.UI;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class GameOverState : BaseState
    {
        private SFXManager _sfxManager;
        
        public GameOverState(GameController gameController, SFXManager sfxManager) : base(gameController)
        {
            _sfxManager = sfxManager;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            PlayerPrefs.SetInt(Consts.PREFS_LAST_WON, 0);
            _gameController.StartCoroutine(ShowPanel());
        }
        
        private IEnumerator ShowPanel()
        {
            yield return new WaitForSeconds(1f);
            _sfxManager.PlayOneShot("Lost");
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene(Consts.SCENE_END_GAME);
        }
    }
}