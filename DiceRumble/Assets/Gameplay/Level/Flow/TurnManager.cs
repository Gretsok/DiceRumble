using UnityEngine;

namespace DR.Gameplay.Level.Flow
{
    public class TurnManager : MonoBehaviour
    {
        private int m_teamTurn;
        public int TeamTurn => m_teamTurn;
        
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
            m_teamTurn = p_teamIndex;
        }
    }
}
