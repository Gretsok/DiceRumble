using System;
using UnityEngine;

namespace DR.MainMenu.TeamCreation
{
    public class TeamMemberSelectionWidget : MonoBehaviour
    {
        public Action<TeamMemberSelectionWidget> OnUpArrowClicked = null;
        public Action<TeamMemberSelectionWidget> OnDownArrowClicked = null;

        public void HandleUpArrowClicked()
        {
            OnUpArrowClicked?.Invoke(this);
        }

        public void HandleDownArrowClicked()
        {
            OnDownArrowClicked?.Invoke(this);
        }
    }
}