using MOtter.StatesMachine;
using UnityEngine;

namespace DR.MainMenu.CreditsScreen
{
    public class CreditsState : FlowState
    {
        [SerializeField]
        private MainMenuGameMode m_gamemode = null;
        private CreditsPanel m_panel = null;
        internal override void RegisterReferences()
        {
            base.RegisterReferences();
            m_panel = GetPanel<CreditsPanel>();
        }
        internal override void RegisterEvents()
        {
            base.RegisterEvents();
            m_panel.BackButton.onClick.AddListener(HandleBackButtonClicked);
        }

        internal override void UnregisterEvents()
        {
            base.UnregisterEvents();
            m_panel.BackButton.onClick.RemoveListener(HandleBackButtonClicked);
        }

        private void HandleBackButtonClicked()
        {
            m_gamemode.SwitchToPreviousState();
        }
    }
}