using UnityEngine;
using Whatwapp.Core.Audio;

namespace Whatwapp.Core.Utils.Executables
{
    public class PlaysSfxExecutable : MonoBehaviour, IExecutable
    {
        [SerializeField] private AudioClip _sfxAudioClip;
        [SerializeField] private string _sfxName;
        
        [SerializeField] private float _volume = 1f;
        
        public void Execute()
        {
            if (string.IsNullOrEmpty(_sfxName) && _sfxAudioClip == null)
            {
                Debug.LogWarning("No SFX to play");
                return;
            }
            
            
            if (_sfxAudioClip != null)
            {
                SFXManager.Instance.PlayOneShot(_sfxAudioClip, _volume);
                return;
            }
            
            SFXManager.Instance.PlayOneShot(_sfxName, _volume);
        }
    }
}