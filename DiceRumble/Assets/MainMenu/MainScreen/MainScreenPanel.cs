using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DR.MainMenu.MainScreen
{
    public class MainScreenPanel : Panel
    {
        [SerializeField]
        private Button m_playButton = null;
        [SerializeField]
        private Button m_quitButton = null;

        public Button PlayButton => m_playButton;
        public Button QuitButton => m_quitButton;

        [SerializeField]
        private GameObject m_defaultSelectedObject = null;

        public override void Show()
        {
            base.Show();
            if (m_defaultSelectedObject != null)
            {
                EventSystem.current.SetSelectedGameObject(m_defaultSelectedObject);
            }
        }
    }
}