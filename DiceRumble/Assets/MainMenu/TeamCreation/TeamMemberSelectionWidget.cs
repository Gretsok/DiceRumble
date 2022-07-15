using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DR.MainMenu.TeamCreation
{
    public class TeamMemberSelectionWidget : MonoBehaviour
    {
        public Action<TeamMemberSelectionWidget> OnUpArrowClicked = null;
        public Action<TeamMemberSelectionWidget> OnDownArrowClicked = null;

        [SerializeField]
        private Image m_dicePreviewImage = null;
        [SerializeField]
        private TextMeshProUGUI m_nameText = null;

        public void HandleUpArrowClicked()
        {
            OnUpArrowClicked?.Invoke(this);
        }

        public void HandleDownArrowClicked()
        {
            OnDownArrowClicked?.Invoke(this);
        }

        public void PopulateWidget(Sprite a_dicePreviewIcon, string a_name)
        {
            m_dicePreviewImage.sprite = a_dicePreviewIcon;
            m_nameText.text = a_name;
        }
    }
}