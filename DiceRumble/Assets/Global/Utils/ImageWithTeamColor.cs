using UnityEngine;
using UnityEngine.UI;

namespace DR.Utils
{
    public class ImageWithTeamColor : MonoBehaviour
    {
        private Image m_image = null;
        [SerializeField]
        private bool m_isFirstTeam = true;
        private void Start()
        {
            m_image = GetComponent<Image>();
            if (m_isFirstTeam)
            {
                m_image.color = (MOtter.MOtt.GM as DRGameManager).GlobalGameData.FirstTeamData.TeamColor;
            }
            else
            {
                m_image.color = (MOtter.MOtt.GM as DRGameManager).GlobalGameData.SecondTeamData.TeamColor;
            }
        }
    }
}