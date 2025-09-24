using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Whatwapp.Core.Audio
{
    public class SFXManager : MonoBehaviour
    {
        [Header("Clips")]
        [SerializeField] private AudioClip[] _clips;
        
        [Header("Settings")]
        [SerializeField] private int _numAudioSources = 5;
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        
        private AudioSource[] _audioSources;
        
        private static SFXManager _instance;
        
        public static SFXManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<SFXManager>();
                if (_instance != null) return _instance;
                var go = new GameObject("SFXManager");
                _instance = go.AddComponent<SFXManager>();
                DontDestroyOnLoad(go);
                return _instance;
            }
        }
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }
            PrepareAudioSources();
        }

        private void PrepareAudioSources()
        {
            _audioSources = new AudioSource[_numAudioSources];
            for (var i = 0; i < _numAudioSources; i++)
            {
                var go = new GameObject($"AudioSource_{i}");
                go.transform.SetParent(transform);
                _audioSources[i] = go.AddComponent<AudioSource>();
                _audioSources[i].outputAudioMixerGroup = _audioMixerGroup;
            }
        }


        public void PlayOneShot(string name, float volume = 1f)
        {
            foreach (var clip in _clips)
            {
                if (clip.name != name) continue;
                PlayOneShot(clip, volume);
                return;
            }
            Debug.LogWarning($"No audio found {name}, check the name correctly");
        }

        public void PlayOneShot(AudioClip clip, float volume = 1f)
        {
            var audioOut = GetAvailableAudioSource();
            if (audioOut == null)
            {
                return;
            }
            // change a bit the pitch to avoid the same sound to be played at the same time
            audioOut.pitch = Random.Range(0.9f, 1.1f);
            audioOut.PlayOneShot(clip, volume);
        }
        
        
        private AudioSource GetAvailableAudioSource()
        {
            return _audioSources.FirstOrDefault(audioSource => !audioSource.isPlaying);
        }
    }
}