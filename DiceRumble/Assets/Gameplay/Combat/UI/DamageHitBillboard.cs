using System.Collections;
using UnityEngine;

namespace DR.Gameplay.Combat.UI
{
    public class DamageHitBillboard : MonoBehaviour
    {
        [SerializeField]
        private float m_lifeTime = 1f;
        [SerializeField]
        private TMPro.TMP_Text m_text = null;

        public void Inflate(int a_damage)
        {
            m_text.text = a_damage.ToString();
            StartCoroutine(LifeRoutine());
        }

        private IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(m_lifeTime);
            Destroy(gameObject);
        }
    }
}