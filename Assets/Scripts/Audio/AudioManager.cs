using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS7.Audio
{
    /// <summary>
    /// Centralised audio manager.
    /// Usage: AudioManager.Instance.PlaySFX("attack_fire"); AudioManager.Instance.PlayMusic("battle_theme");
    /// Wire AudioClipCatalogue ScriptableObject in Inspector.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Catalogue")]
        public AudioClipCatalogue catalogue;

        [Header("Sources")]
        public AudioSource musicSourceA;
        public AudioSource musicSourceB;
        [Range(0f, 1f)] public float masterVolume  = 1f;
        [Range(0f, 1f)] public float sfxVolume     = 0.85f;
        [Range(0f, 1f)] public float musicVolume   = 0.6f;

        [Header("SFX Pool")]
        [Tooltip("Number of pooled AudioSources for overlapping SFX")]
        public int sfxPoolSize = 12;

        private AudioSource[] _sfxPool;
        private int           _poolIndex;
        private AudioSource   _activeMusicSource;
        private AudioSource   _idleMusicSource;

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            BuildPool();

            _activeMusicSource = musicSourceA;
            _idleMusicSource   = musicSourceB;
        }

        private void BuildPool()
        {
            _sfxPool = new AudioSource[sfxPoolSize];
            for (int i = 0; i < sfxPoolSize; i++)
            {
                var go = new GameObject($"SFX_Pool_{i}");
                go.transform.SetParent(transform);
                _sfxPool[i] = go.AddComponent<AudioSource>();
                _sfxPool[i].playOnAwake = false;
            }
        }

        // ── SFX ───────────────────────────────────────────────────────────────
        /// <summary>Plays a one-shot SFX by key in the catalogue.</summary>
        public void PlaySFX(string key, float pitchVariance = 0.05f)
        {
            if (catalogue == null) return;
            var clip = catalogue.GetSFX(key);
            if (clip == null) { Debug.LogWarning($"[Audio] SFX not found: {key}"); return; }

            var src = NextPoolSource();
            src.clip   = clip;
            src.volume = sfxVolume * masterVolume;
            src.pitch  = 1f + UnityEngine.Random.Range(-pitchVariance, pitchVariance);
            src.Play();
        }

        /// <summary>Plays a positional SFX at world position (3D falloff).</summary>
        public void PlaySFXAt(string key, Vector3 position, float spatialBlend = 0.8f)
        {
            if (catalogue == null) return;
            var clip = catalogue.GetSFX(key);
            if (clip == null) return;
            AudioSource.PlayClipAtPoint(clip, position, sfxVolume * masterVolume);
        }

        private AudioSource NextPoolSource()
        {
            var src = _sfxPool[_poolIndex];
            _poolIndex = (_poolIndex + 1) % sfxPoolSize;
            return src;
        }

        // ── Music ─────────────────────────────────────────────────────────────
        /// <summary>Cross-fades to a new music track by key.</summary>
        public void PlayMusic(string key, float fadeDuration = 1.5f)
        {
            if (catalogue == null) return;
            var clip = catalogue.GetMusic(key);
            if (clip == null) { Debug.LogWarning($"[Audio] Music not found: {key}"); return; }

            if (_activeMusicSource.clip == clip) return; // already playing

            _idleMusicSource.clip   = clip;
            _idleMusicSource.volume = 0f;
            _idleMusicSource.loop   = true;
            _idleMusicSource.Play();

            StartCoroutine(CrossFade(fadeDuration));
        }

        private IEnumerator CrossFade(float duration)
        {
            float elapsed = 0f;
            float targetVol = musicVolume * masterVolume;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                _activeMusicSource.volume = Mathf.Lerp(targetVol, 0f, t);
                _idleMusicSource.volume   = Mathf.Lerp(0f, targetVol, t);
                yield return null;
            }

            _activeMusicSource.Stop();
            (_activeMusicSource, _idleMusicSource) = (_idleMusicSource, _activeMusicSource);
        }

        public void StopMusic(float fadeDuration = 1f)
            => StartCoroutine(FadeOut(_activeMusicSource, fadeDuration));

        private IEnumerator FadeOut(AudioSource src, float duration)
        {
            float startVol = src.volume;
            float elapsed  = 0f;
            while (elapsed < duration)
            {
                elapsed    += Time.deltaTime;
                src.volume  = Mathf.Lerp(startVol, 0f, elapsed / duration);
                yield return null;
            }
            src.Stop();
        }

        // ── Volume Control ────────────────────────────────────────────────────
        public void SetMasterVolume(float vol)
        {
            masterVolume = Mathf.Clamp01(vol);
            _activeMusicSource.volume = musicVolume * masterVolume;
        }
    }

    // ── Catalogue ScriptableObject ─────────────────────────────────────────────
    [CreateAssetMenu(menuName = "DS7/Audio Clip Catalogue", fileName = "AudioClipCatalogue")]
    public class AudioClipCatalogue : ScriptableObject
    {
        [System.Serializable]
        public struct Entry
        {
            public string    key;
            public AudioClip clip;
        }

        [Header("SFX")]
        public Entry[] sfx;

        [Header("Music")]
        public Entry[] music;

        private Dictionary<string, AudioClip> _sfxMap;
        private Dictionary<string, AudioClip> _musicMap;

        private void OnEnable()
        {
            _sfxMap   = new Dictionary<string, AudioClip>();
            _musicMap = new Dictionary<string, AudioClip>();
            foreach (var e in sfx)   _sfxMap[e.key]   = e.clip;
            foreach (var e in music) _musicMap[e.key] = e.clip;
        }

        public AudioClip GetSFX(string key)   => _sfxMap != null && _sfxMap.TryGetValue(key, out var c) ? c : null;
        public AudioClip GetMusic(string key) => _musicMap != null && _musicMap.TryGetValue(key, out var c) ? c : null;
    }
}
