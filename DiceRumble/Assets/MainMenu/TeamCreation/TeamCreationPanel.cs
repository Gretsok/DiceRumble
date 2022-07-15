using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DR.MainMenu.TeamCreation
{
    public class TeamCreationPanel : Panel
    {
        [SerializeField]
        private GameObject m_defaultSelectedObject = null;

        [SerializeField]
        private TeamCreationWidget m_firstTeamCreationWidget = null;
        [SerializeField]
        private TeamCreationWidget m_secondTeamCreationWidget = null;
        public TeamCreationWidget FirstTeamCreationWidget => m_firstTeamCreationWidget;
        public TeamCreationWidget SecondTeamCreationWidget => m_secondTeamCreationWidget;

        [SerializeField]
        private Button m_startGameButton = null;
        [SerializeField]
        private Button m_backButton = null;
        public Button StartGameButton => m_startGameButton;
        public Button BackButton => m_backButton;

        public override void Show()
        {
            base.Show();
            if(m_defaultSelectedObject != null)
            {
                EventSystem.current.SetSelectedGameObject(m_defaultSelectedObject);
            }
        }
    }
}