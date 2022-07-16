using UnityEngine;

namespace DR.Gameplay.Combat.UI
{
    public class DamageHitBillboardSpawner : MonoBehaviour
    {
        [SerializeField]
        private DamageHitBillboard m_hitPrefab = null;
        [SerializeField]
        private CombatController m_combatController = null;

        private void Start()
        {
            m_combatController.OnDamageTaken += HandleDamageTaken;
        }

        private void OnDestroy()
        {
            m_combatController.OnDamageTaken -= HandleDamageTaken;
        }

        private void HandleDamageTaken(CombatController arg1, int arg2)
        {
            ShowHit(arg2);
        }

        public void ShowHit(int a_damage)
        {
            var newHit = Instantiate(m_hitPrefab, transform);
            newHit.transform.localPosition = Vector3.zero;
            newHit.Inflate(a_damage);
        }
    }
}