using UnityEngine;

namespace DR.Utils
{
    public class TextWithTeamColor : MonoBehaviour
    {
        private TMPro.TMP_Text m_text = null;
        [SerializeField]
        private bool m_isFirstTeam = true;
        private void Start()
        {
            m_text = GetComponent<TMPro.TMP_Text>();
            if(m_isFirstTeam)
            {
                m_text.color = (MOtter.MOtt.GM as DRGameManager).GlobalGameData.FirstTeamData.TeamColor;
            }
            else
            {
                m_text.color = (MOtter.MOtt.GM as DRGameManager).GlobalGameData.SecondTeamData.TeamColor;
            }
        }
    }
}