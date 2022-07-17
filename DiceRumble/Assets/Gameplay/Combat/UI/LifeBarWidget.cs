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
        [SerializeField] private Color m_firstTeamHealthBarColor;
        [SerializeField] private Color m_firstTeamHealthTextBackgroundColor;
        [SerializeField] private Color m_secondTeamHealthBarColor;
        [SerializeField] private Color m_secondTeamHealthTextBackgroundColor;

        private void Start()
        {
            m_combatController.OnLifeUpdated += HandleLifeUpdated;
            HandleLifeUpdated(m_combatController);
        }

        public void SetupTeamColor(int p_teamIndex)
        {
            m_healthBarBackground.color = p_teamIndex == 0 ? m_firstTeamHealthBarColor : m_secondTeamHealthBarColor;
            m_healthTextBackground.color = p_teamIndex == 0 ? m_firstTeamHealthTextBackgroundColor : m_secondTeamHealthTextBackgroundColor;
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