using UnityEngine;

namespace DR.Gameplay.Level.Flow
{
    public class TurnManager : MonoBehaviour
    {
        private int m_turnTeam;
        public int TurnTeam => m_turnTeam;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetTeamTurn(int p_teamIndex)
        {
            m_turnTeam = p_teamIndex;
        }
    }
}
