using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MOtter.SoundManagement
{
    public class SoundManager : MonoBehaviour
    {
        private float m_musicVolume = 1f;
        private float m_sfxVolume = 1f;

        [SerializeField, Tooltip("")]
        private AudioSource m_audioSource = null;

        private List<AudioSource> m_audioSourcesPool = new List<AudioSource>();

        private List<AudioSource> m_musicAudioSourcesPlaying = new List<AudioSource>();
        private List<AudioSource> m_sfxAudioSourcesPlaying = new List<AudioSource>();

        public AudioSource Play2DSound(SoundData soundData, bool loop = false, float volume = 1f)
        {
            if(soundData.AudioClip == null)
            {
                Debug.LogError($"No audio clip in Sound Data : {soundData.name}");
                return null;
            }
            CheckFreeingRoutine();
            AudioSource audioSource = GetFreeAudioSource();
            audioSource.loop = loop;
            audioSource.clip = soundData.AudioClip;
            audioSource.volume = Mathf.Clamp01(volume) * GetVolume(soundData.CategoryName);
            audioSource.name = soundData.AudioClip.name;
            audioSource.spatialBlend = 0f;
            audioSource.Play();

            m_audioSourcesPool.Remove(audioSource);

            if(ESoundCategoryName.Music == soundData.CategoryName)
            {
                m_musicAudioSourcesPlaying.Add(audioSource);
            }
            else if(ESoundCategoryName.SFX == soundData.CategoryName)
            {
                m_sfxAudioSourcesPlaying.Add(audioSource);
            }

            return audioSource;
        }

        public AudioSource Play3DSound(SoundData soundData, Vector3 position, bool loop = false, float volume = 1f, Transform parent = null, float spatialBlend = 1f)
        {
            if (soundData.AudioClip == null)
            {
                Debug.LogError($"No audio clip in Sound Data : {soundData.name}");
                return null;
            }
            CheckFreeingRoutine();
            AudioSource audioSource = GetFreeAudioSource();
            audioSource.loop = loop;
            audioSource.clip = soundData.AudioClip;
            audioSource.volume = Mathf.Clamp01(volume) * GetVolume(soundData.CategoryName);
            audioSource.name = soundData.AudioClip.name;
            audioSource.transform.position = position;
            audioSource.spatialBlend = spatialBlend;
            if (parent != null)
            {
                audioSource.transform.SetParent(parent);
            }
            audioSource.Play();

            m_audioSourcesPool.Remove(audioSource);

            if (ESoundCategoryName.Music == soundData.CategoryName)
            {
                m_musicAudioSourcesPlaying.Add(audioSource);
            }
            else if (ESoundCategoryName.SFX == soundData.CategoryName)
            {
                m_sfxAudioSourcesPlaying.Add(audioSource);
            }

            return audioSource;
        }

        public AudioSource GetFreeAudioSource()
        {
            AudioSource audioSource = null;

            if(m_audioSourcesPool.Count > 0)
            {
                audioSource = m_audioSourcesPool[0];
            }
            else
            {
                audioSource = Instantiate(m_audioSource, this.transform);
            }

            return audioSource;
        }

        #region Private Methods
        private Coroutine m_freeingRoutine = null;
        /// <summary>
        /// Start a coroutine that checks when audio source are freed, if not already started
        /// </summary>
        private void CheckFreeingRoutine()
        {
            if(m_freeingRoutine == null)
            {
                m_freeingRoutine = StartCoroutine(PoolFreeAudioSourcesRoutine());
            }
        }

        private IEnumerator PoolFreeAudioSourcesRoutine()
        {
            yield return null;
            int musicIndex = 0;
            int soundIndex = 0;
            while(true)
            {
                if(m_musicAudioSourcesPlaying.Count > 0)
                {
                    if(m_musicAudioSourcesPlaying.Count - 1 < musicIndex)
                    {
                        musicIndex = 0;
                    }

                    if(!m_musicAudioSourcesPlaying[musicIndex].isPlaying)
                    {
                        m_musicAudioSourcesPlaying[musicIndex].transform.parent = transform;
                        m_audioSourcesPool.Add(m_musicAudioSourcesPlaying[musicIndex]);
                        m_musicAudioSourcesPlaying.RemoveAt(musicIndex);
                    }
                    musicIndex++;
                }

                if (m_sfxAudioSourcesPlaying.Count > 0)
                {
                    if (m_sfxAudioSourcesPlaying.Count - 1 < soundIndex)
                    {
                        soundIndex = 0;
                    }

                    if (!m_sfxAudioSourcesPlaying[soundIndex].isPlaying)
                    {
                        m_sfxAudioSourcesPlaying[soundIndex].transform.parent = transform;
                        m_audioSourcesPool.Add(m_sfxAudioSourcesPlaying[soundIndex]);
                        m_sfxAudioSourcesPlaying.RemoveAt(soundIndex);
                    }
                    soundIndex++;
                }
                yield return null;
            }
        }
        #endregion


        #region Volume Management
        public float GetVolume(ESoundCategoryName soundCategory)
        {
            switch(soundCategory)
            {
                case ESoundCategoryName.Music:

                    return m_musicVolume;
                case ESoundCategoryName.SFX:
                    return m_sfxVolume;
                default:
                    Debug.LogError("Invalid sound category");
                    return -1f;
            }
        }

        public void SetVolume(float volume, ESoundCategoryName soundCategory)
        {
            switch (soundCategory)
            {
                case ESoundCategoryName.Music:
                    m_musicVolume = Mathf.Clamp01(volume);
                    m_musicAudioSourcesPlaying.ForEach((x) => x.volume = m_musicVolume);
                    break;
                case ESoundCategoryName.SFX:
                    m_sfxVolume = Mathf.Clamp01(volume);
                    m_sfxAudioSourcesPlaying.ForEach((x) => x.volume = m_sfxVolume);
                    break;
                default:
                    Debug.LogError("Invalid sound category");
                    break;
            }
        }
        #endregion

    }

    public enum ESoundCategoryName
    {
        Music,
        SFX
    }
}