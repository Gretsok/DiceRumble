using MOtter.StatesMachine;
using UnityEngine;

namespace DR.MainMenu.MainScreen
{
    public class MainScreenState : FlowState
    {
        [SerializeField]
        private MainMenuGameMode m_gamemode = null;

        private MainScreenPanel m_panel = null;

        internal override void RegisterReferences()
        {
            base.RegisterReferences();
            m_panel = GetPanel<MainScreenPanel>();
        }

        internal override void RegisterEvents()
        {
            base.RegisterEvents();
            m_panel.PlayButton.onClick.AddListener(HandlePlayButtonClicked);
            m_panel.QuitButton.onClick.AddListener(HandleQuitButtonClicked);
        }

        internal override void UnregisterEvents()
        {
            base.UnregisterEvents();
            m_panel.PlayButton.onClick.RemoveListener(HandlePlayButtonClicked);
            m_panel.QuitButton.onClick.RemoveListener(HandleQuitButtonClicked);
        }

        private void HandleQuitButtonClicked()
        {
            Application.Quit();
        }

        private void HandlePlayButtonClicked()
        {
            m_gamemode.SwitchToNextState();
        }
    }
}