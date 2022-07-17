using MOtter.StatesMachine;
using System;
using UnityEngine;

namespace DR.MainMenu.MainScreen
{
    public class MainScreenState : FlowState
    {
        [SerializeField]
        private MainMenuGameMode m_gamemode = null;

        private MainScreenPanel m_panel = null;

        [SerializeField]
        private FlowState m_creditsState = null;

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
            m_panel.CreditsButton.onClick.AddListener(HandleCreditsButtonClicked);
        }



        internal override void UnregisterEvents()
        {
            base.UnregisterEvents();
            m_panel.PlayButton.onClick.RemoveListener(HandlePlayButtonClicked);
            m_panel.QuitButton.onClick.RemoveListener(HandleQuitButtonClicked);
            m_panel.CreditsButton.onClick.RemoveListener(HandleCreditsButtonClicked);
        }

        private void HandleQuitButtonClicked()
        {
            Application.Quit();
        }

        private void HandlePlayButtonClicked()
        {
            m_gamemode.SwitchToNextState();
        }

        private void HandleCreditsButtonClicked()
        {
            m_gamemode.SwitchToState(m_creditsState);
        }
    }
}