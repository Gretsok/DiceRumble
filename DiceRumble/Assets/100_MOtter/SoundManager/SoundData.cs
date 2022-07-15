using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MOtter.SoundManagement
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "Sound/SoundData")]
    public class SoundData : ScriptableObject
    {
        [SerializeField]
        private List<AudioClip> m_audioClips = null;

        [SerializeField]
        private ESoundCategoryName m_categoryName = ESoundCategoryName.SFX;

        public AudioClip AudioClip { 
            get
            {
                if(m_audioClips.Count > 0)
                {
                    int randomIndex = Random.Range(0, m_audioClips.Count);
                    return m_audioClips[randomIndex];
                }
                return null;
            }
        }
            

        public ESoundCategoryName CategoryName => m_categoryName;
    }
}