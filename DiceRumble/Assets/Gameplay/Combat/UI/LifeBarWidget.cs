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

        private void Start()
        {
            m_combatController.OnLifeUpdated += HandleLifeUpdated;
            HandleLifeUpdated(m_combatController);
        }

        private void HandleLifeUpdated(CombatController obj)
        {
            m_filler.fillAmount = (float)obj.CurrentHealth / (float)obj.BaseHealth;
        }

        private void OnDestroy()
        {
            m_combatController.OnLifeUpdated -= HandleLifeUpdated;
        }
    }
}