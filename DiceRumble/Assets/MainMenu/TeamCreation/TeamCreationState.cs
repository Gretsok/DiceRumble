using MOtter.LevelData;
using MOtter.StatesMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DR.MainMenu.TeamCreation
{
    public class TeamCreationState : FlowState
    {
        [System.Serializable]
        private struct TeamChoices
        {
            public List<int> MemberIndexes;
        }
        [SerializeField]
        private MainMenuGameMode m_gamemode = null;
        [SerializeField]
        private LevelData m_gameLevelData = null;
        private bool m_hasLaunchedTheGame = false;

        private TeamCreationPanel m_panel = null;

        [SerializeField]
        private AvailableDicesData m_availableDicesData = null;

        private List<TeamChoices> m_teamChoices = new List<TeamChoices>();



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

        internal override void SetUpDependencies()
        {
            base.SetUpDependencies();

            m_panel.FirstTeamCreationWidget.InflateBannerColor((MOtter.MOtt.GM as DRGameManager).GlobalGameData.FirstTeamData.TeamColor);
            m_panel.SecondTeamCreationWidget.InflateBannerColor((MOtter.MOtt.GM as DRGameManager).GlobalGameData.SecondTeamData.TeamColor);

            while (m_teamChoices.Count < 2)
            {
                TeamChoices teamChoices;
                teamChoices.MemberIndexes = new List<int>();
                teamChoices.MemberIndexes.Add(0);
                teamChoices.MemberIndexes.Add(0);
                teamChoices.MemberIndexes.Add(0);
                m_teamChoices.Add(teamChoices);
            }
            PopulateWidgets();
        }

        private void HandleBackButtonClicked()
        {
            m_gamemode.SwitchToPreviousState();
        }

        private void HandleStartGameButtonClicked()
        {
            if (m_hasLaunchedTheGame) return;
            // Debug.Log("Starting game");

            Gameplay.Teams.TeamsDataConveyor teamsDataConveyor = new Gameplay.Teams.TeamsDataConveyor();
            teamsDataConveyor.FirstTeamDicesData = new List<Gameplay.Dices.DiceData>();
            for(int i = 0; i < m_teamChoices[0].MemberIndexes.Count; ++i)
            {
                teamsDataConveyor.FirstTeamDicesData.Add(m_availableDicesData.AvailableDicesList[m_teamChoices[0].MemberIndexes[i]]);
            }
            teamsDataConveyor.SecondTeamDicesData = new List<Gameplay.Dices.DiceData>();
            for(int i = 0; i < m_teamChoices[1].MemberIndexes.Count; ++i)
            {
                teamsDataConveyor.SecondTeamDicesData.Add(m_availableDicesData.AvailableDicesList[m_teamChoices[1].MemberIndexes[i]]);
            }
            MOtter.MOtt.DATACONVEY.RegisterContainer(teamsDataConveyor);

            m_gameLevelData.LoadLevel();
            m_hasLaunchedTheGame = true;
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
                // Debug.Log($"Press down arrow of member {arg1 + 1} of first team");
                m_teamChoices[0].MemberIndexes[arg1]--;
                if(m_teamChoices[0].MemberIndexes[arg1] < 0)
                {
                    m_teamChoices[0].MemberIndexes[arg1] = m_availableDicesData.AvailableDicesList.Count - 1;
                }
            }
            else if(arg0 == m_panel.SecondTeamCreationWidget)
            {
                // Debug.Log($"Press down arrow of member {arg1 + 1} of second team");
                m_teamChoices[1].MemberIndexes[arg1]--;
                if (m_teamChoices[1].MemberIndexes[arg1] < 0)
                {
                    m_teamChoices[1].MemberIndexes[arg1] = m_availableDicesData.AvailableDicesList.Count - 1;
                }
            }
            PopulateWidgets();
        }

        private void HandleUpArrowClicked(TeamCreationWidget arg0, int arg1)
        {
            if (arg0 == m_panel.FirstTeamCreationWidget)
            {
                // Debug.Log($"Press up arrow of member {arg1 + 1} of first team");
                m_teamChoices[0].MemberIndexes[arg1]++;
                m_teamChoices[0].MemberIndexes[arg1] = m_teamChoices[0].MemberIndexes[arg1] % m_availableDicesData.AvailableDicesList.Count;
            }
            else if (arg0 == m_panel.SecondTeamCreationWidget)
            {
                // Debug.Log($"Press up arrow of member {arg1 + 1} of second team");
                m_teamChoices[1].MemberIndexes[arg1]++;
                m_teamChoices[1].MemberIndexes[arg1] = m_teamChoices[1].MemberIndexes[arg1] % m_availableDicesData.AvailableDicesList.Count;
            }
            PopulateWidgets();
        }

        public void PopulateWidgets()
        {
            for(int i = 0; i < m_panel.FirstTeamCreationWidget.TeamMemberSelectionWidgets.Count; ++i)
            {
                Gameplay.Dices.DiceData diceData = m_availableDicesData.AvailableDicesList[m_teamChoices[0].MemberIndexes[i]];
                m_panel.FirstTeamCreationWidget.TeamMemberSelectionWidgets[i].PopulateWidget(diceData.DicePreviewIcon, diceData.DiceName);
            }

            for (int i = 0; i < m_panel.SecondTeamCreationWidget.TeamMemberSelectionWidgets.Count; ++i)
            {
                Gameplay.Dices.DiceData diceData = m_availableDicesData.AvailableDicesList[m_teamChoices[1].MemberIndexes[i]];
                m_panel.SecondTeamCreationWidget.TeamMemberSelectionWidgets[i].PopulateWidget(diceData.DicePreviewIcon, diceData.DiceName);
            }
        }
    }
}