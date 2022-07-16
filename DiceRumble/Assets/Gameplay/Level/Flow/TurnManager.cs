using UnityEngine;

namespace DR.Gameplay.Level.Flow
{
    public class TurnManager : MonoBehaviour
    {
        private int m_turnTeam;
        public int TurnTeam => m_turnTeam;

        public void SetTeamTurn(int p_teamIndex)
        {
            m_turnTeam = p_teamIndex;
        }

        public void ChangeTurn()
        {
            m_turnTeam = 1 - m_turnTeam;
        }
    }
}
