using System.Collections.Generic;
using UnityEngine;

namespace DR.MainMenu.TeamCreation
{
    public class TeamCreationWidget : MonoBehaviour
    {
        [SerializeField]
        private List<TeamMemberSelectionWidget> m_teamMembreSelectionWidgets = null;
        public List<TeamMemberSelectionWidget> TeamMemberSelectionWidgets => m_teamMembreSelectionWidgets;

        private UnityEngine.Events.UnityAction<TeamCreationWidget, int> OnUpArrowClicked = null;
        private UnityEngine.Events.UnityAction<TeamCreationWidget, int> OnDownArrowClicked = null;

        public void RegisterArrowsCallback(UnityEngine.Events.UnityAction<TeamCreationWidget, int> a_upArrowClicked,
            UnityEngine.Events.UnityAction<TeamCreationWidget, int> a_downArrowClicked)
        {
            OnUpArrowClicked = a_upArrowClicked;
            OnDownArrowClicked = a_downArrowClicked;

            m_teamMembreSelectionWidgets.ForEach(x => x.OnUpArrowClicked += HandleUpArrowClicked);
            m_teamMembreSelectionWidgets.ForEach(x => x.OnDownArrowClicked += HandleDownArrowClicked);
        }

        private void HandleDownArrowClicked(TeamMemberSelectionWidget obj)
        {
            OnDownArrowClicked?.Invoke(this, m_teamMembreSelectionWidgets.FindIndex(x => x == obj));
        }

        private void HandleUpArrowClicked(TeamMemberSelectionWidget obj)
        {
            OnUpArrowClicked?.Invoke(this, m_teamMembreSelectionWidgets.FindIndex(x => x == obj));
        }

        public void UnregisterArrowsCallback(UnityEngine.Events.UnityAction<TeamCreationWidget, int> a_upArrowClicked,
            UnityEngine.Events.UnityAction<TeamCreationWidget, int> a_downArrowClicked)
        {
            m_teamMembreSelectionWidgets.ForEach(x => x.OnUpArrowClicked -= HandleUpArrowClicked);
            m_teamMembreSelectionWidgets.ForEach(x => x.OnDownArrowClicked -= HandleDownArrowClicked);
            OnUpArrowClicked = null;
            OnDownArrowClicked = null;
        }
    }
}