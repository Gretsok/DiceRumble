using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DR.Gameplay.Combat.UI
{
    public class LifeBarWidget : MonoBehaviour
    {
        [SerializeField]
        private CombatController m_combatController = null;
        [SerializeField]
        private Image m_filler = null;
        [SerializeField] private Image m_healthBarBackground = null;
        [SerializeField] private RawImage m_healthTextBackground = null;
        [SerializeField] private TextMeshProUGUI m_healthText;

        private void Start()
        {
            m_combatController.OnLifeUpdated += HandleLifeUpdated;
            HandleLifeUpdated(m_combatController);
        }

        public void SetupTeamColor(int p_teamIndex)
        {
            m_healthBarBackground.color = p_teamIndex == 0 ? 
                (MOtter.MOtt.GM as DRGameManager).GlobalGameData.FirstTeamData.TeamColor 
                : (MOtter.MOtt.GM as DRGameManager).GlobalGameData.SecondTeamData.TeamColor;
            m_healthTextBackground.color = p_teamIndex == 0 ?
                (MOtter.MOtt.GM as DRGameManager).GlobalGameData.FirstTeamData.TeamColor
                : (MOtter.MOtt.GM as DRGameManager).GlobalGameData.SecondTeamData.TeamColor;
        }

        private void HandleLifeUpdated(CombatController obj)
        {
            m_filler.fillAmount = (float)obj.CurrentHealth / (float)obj.BaseHealth;
            m_healthText.text = obj.CurrentHealth.ToString();
        }

        private void OnDestroy()
        {
            m_combatController.OnLifeUpdated -= HandleLifeUpdated;
        }
    }
}