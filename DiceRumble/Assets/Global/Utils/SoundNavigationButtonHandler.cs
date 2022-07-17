using MOtter.SoundManagement;
using UnityEngine;

namespace DR.Utils
{
    public class SoundNavigationButtonHandler : MonoBehaviour
    {
        [SerializeField]
        private SoundData m_selectedSound = null;
        [SerializeField]
        private SoundData m_submittedSound = null;


        public void HandleButtonSubmitted()
        {
            MOtter.MOtt.SOUND.Play2DSound(m_submittedSound);
        }

        public void HandleButtonSelected()
        {
            MOtter.MOtt.SOUND.Play2DSound(m_selectedSound);
        }
    }
}