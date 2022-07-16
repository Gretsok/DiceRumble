using System.Collections;
using TMPro;
using UnityEngine;

namespace DR.Gameplay.Level.Flow
{
    public class TurnPanel : Panel
    {
        [SerializeField]
        private GameObject m_firstTeamPlayingContainer = null;
        [SerializeField]
        private GameObject m_secondTeamPlayingContainer = null;
        [SerializeField]
        private float m_durationToShowTeam = 4f;

        [SerializeField]
        private TextMeshProUGUI m_movementLeft = null;

        public void ShowTeamToPlay(bool a_isFirstTeam)
        {
            m_firstTeamPlayingContainer.SetActive(a_isFirstTeam);
            m_secondTeamPlayingContainer.SetActive(!a_isFirstTeam);
            StartCoroutine(ShowingTeamRoutine());

        }
        private IEnumerator ShowingTeamRoutine()
        {
            yield return new WaitForSeconds(m_durationToShowTeam);
            m_firstTeamPlayingContainer.SetActive(false);
            m_secondTeamPlayingContainer.SetActive(false);
        }

        public void ShowMovementLeft(int a_movementLeft)
        {
            if(a_movementLeft > 1)
            {
                m_movementLeft.text = $"{a_movementLeft} movements left";
            }
            else if(a_movementLeft == 1)
            {
                m_movementLeft.text = $"Only one movement left";
            }
            else
            {
                m_movementLeft.text = $"No movement left";
            }
        }
    }
}