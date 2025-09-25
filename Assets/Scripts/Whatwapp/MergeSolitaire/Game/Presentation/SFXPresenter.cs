using UnityEngine;
using Whatwapp.Core.Audio;

namespace Whatwapp.MergeSolitaire.Game.Presentation
{
    public class SFXPresenter : MonoBehaviour, ISFXPresenter
    {
        private SFXManager _sfxManager;

        public void Initialize(SFXManager sfxManager)
        {
            _sfxManager = sfxManager;
        }

        public void PlayOneShot(string name, float volume = 1f)
        {
            _sfxManager.PlayOneShot(name, volume);
        }
    }
}