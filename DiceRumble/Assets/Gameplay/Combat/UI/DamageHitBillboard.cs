using System.Collections;
using DR.Gameplay.Dices;
using UnityEngine;

namespace DR.Gameplay.Combat.UI
{
    public class DamageHitBillboard : MonoBehaviour
    {
        [SerializeField]
        private float m_lifeTime = 1f;
        [SerializeField]
        private TMPro.TMP_Text m_text = null;

        [SerializeField] private Color m_fireColor;
        [SerializeField] private Color m_plantColor;
        [SerializeField] private Color m_poisonColor;
        [SerializeField] private Color m_rockColor;
        [SerializeField] private Color m_waterColor;

        public void Inflate(int a_damage, EDiceType p_damageType)
        {
            Color color = m_text.color;
            switch (p_damageType)
            {
                case EDiceType.Neutral:
                    color = m_text.color;
                    break;
                case EDiceType.Fire:
                    color = m_fireColor;
                    break;
                case EDiceType.Plant:
                    color = m_plantColor;
                    break;
                case EDiceType.Poison:
                    color = m_poisonColor;
                    break;
                case EDiceType.Rock:
                    color = m_rockColor;
                    break;
                case EDiceType.Water:
                    color = m_waterColor;
                    break;
            }
            m_text.text = a_damage.ToString();
            m_text.color = color;
            StartCoroutine(LifeRoutine());
        }

        private IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(m_lifeTime);
            Destroy(gameObject);
        }
    }
}