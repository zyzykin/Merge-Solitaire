using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Whatwapp.Core.Audio
{
    public class SFXManager : MonoBehaviour
    {
        [Header("Clips")]
        [SerializeField] private AudioClip[] clips;

        [Header("Settings")]
        [SerializeField] private int initialPoolSize = 5;
        [SerializeField] private int maxPoolSize = 20;
        [SerializeField] private AudioMixerGroup audioMixerGroup;

        private List<AudioSource> _audioSourcePool;

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

            InitializeAudioSourcePool();
        }

        private void InitializeAudioSourcePool()
        {
            _audioSourcePool = new List<AudioSource>();
            for (var i = 0; i < initialPoolSize; i++)
            {
                _audioSourcePool.Add(CreateNewAudioSource());
            }
        }

        private AudioSource CreateNewAudioSource()
        {
            var go = new GameObject($"AudioSource_{_audioSourcePool.Count}");
            go.transform.SetParent(transform);
            var audioSource = go.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            return audioSource;
        }

        public void PlayOneShot(string name, float volume = 1f)
        {
            foreach (var clip in clips)
            {
                if (clip.name != name) continue;
                PlayOneShot(clip, volume);
                return;
            }

            Debug.LogWarning($"No audio found {name}, check the name correctly");
        }

        public void PlayOneShot(AudioClip clip, float volume = 1f)
        {
            var audioSource = GetAvailableAudioSource();
            if (audioSource == null)
            {
                Debug.LogWarning("No available AudioSource in pool");
                return;
            }

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clip, volume);
        }

        private AudioSource GetAvailableAudioSource()
        {
            var availableSource = _audioSourcePool.FirstOrDefault(source => !source.isPlaying);

            if (availableSource == null && _audioSourcePool.Count < maxPoolSize)
            {
                availableSource = CreateNewAudioSource();
                _audioSourcePool.Add(availableSource);
            }

            return availableSource;
        }
    }
}