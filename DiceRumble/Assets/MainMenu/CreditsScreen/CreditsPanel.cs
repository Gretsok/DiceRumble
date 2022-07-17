using UnityEngine;
using UnityEngine.UI;

namespace DR.MainMenu.CreditsScreen
{
    public class CreditsPanel : Panel
    {
        [SerializeField]
        private Button m_backButton = null;
        public Button BackButton => m_backButton;
    }
}