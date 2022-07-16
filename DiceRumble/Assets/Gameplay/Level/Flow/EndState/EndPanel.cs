using TMPro;
using UnityEngine;

namespace DR.Gameplay.Level.Flow.EndState
{
    public class EndPanel : Panel
    {
        [SerializeField]
        private TextMeshProUGUI m_text = null;
        public void InflateWinningTeam(int a_winningTeam)
        {
            m_text.text = $"Team {a_winningTeam + 1} won!";
        }
    }
}