using MOtter.StatesMachine;
using UnityEngine;

namespace DR.MainMenu.TeamCreation
{
    public class TeamCreationState : FlowState
    {
        [SerializeField]
        private MainMenuGameMode m_gamemode = null;

        private TeamCreationPanel m_panel = null;

        internal override void RegisterReferences()
        {
            base.RegisterReferences();
            m_panel = GetPanel<TeamCreationPanel>();
        }

        internal override void RegisterEvents()
        {
            base.RegisterEvents();
            m_panel.FirstTeamCreationWidget.RegisterArrowsCallback(HandleUpArrowClicked, HandleDownArrowClicked);
            m_panel.SecondTeamCreationWidget.RegisterArrowsCallback(HandleUpArrowClicked, HandleDownArrowClicked);
            m_panel.StartGameButton.onClick.AddListener(HandleStartGameButtonClicked);
            m_panel.BackButton.onClick.AddListener(HandleBackButtonClicked);
        }

        private void HandleBackButtonClicked()
        {
            m_gamemode.SwitchToPreviousState();
        }

        private void HandleStartGameButtonClicked()
        {
            Debug.Log("Starting game");
        }

        internal override void UnregisterEvents()
        {
            base.UnregisterEvents();
            m_panel.FirstTeamCreationWidget.UnregisterArrowsCallback(HandleUpArrowClicked, HandleDownArrowClicked);
            m_panel.SecondTeamCreationWidget.UnregisterArrowsCallback(HandleUpArrowClicked, HandleDownArrowClicked);
            m_panel.StartGameButton.onClick.RemoveListener(HandleStartGameButtonClicked);
            m_panel.BackButton.onClick.RemoveListener(HandleBackButtonClicked);
        }

        private void HandleDownArrowClicked(TeamCreationWidget arg0, int arg1)
        {
            if(arg0 == m_panel.FirstTeamCreationWidget)
            {
                Debug.Log($"Press down arrow of member {arg1 + 1} of first team");
            }
            else if(arg0 == m_panel.SecondTeamCreationWidget)
            {
                Debug.Log($"Press down arrow of member {arg1 + 1} of second team");
            }
        }

        private void HandleUpArrowClicked(TeamCreationWidget arg0, int arg1)
        {
            if (arg0 == m_panel.FirstTeamCreationWidget)
            {
                Debug.Log($"Press up arrow of member {arg1 + 1} of first team");
            }
            else if (arg0 == m_panel.SecondTeamCreationWidget)
            {
                Debug.Log($"Press up arrow of member {arg1 + 1} of second team");
            }
        }
    }
}