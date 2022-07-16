using UnityEngine;

namespace DR.Utils
{
    public class StickToTransformPosition : MonoBehaviour
    {
        [SerializeField]
        private Transform m_target = null;
        [SerializeField]
        private bool m_keepOffset = true;
        private Vector3 m_offset = default;

        private void Start()
        {
            if (m_keepOffset)
                m_offset = transform.position - m_target.position;
            else
                m_offset = Vector3.zero;
        }

        private void LateUpdate()
        {
            transform.position = m_target.transform.position + m_offset;
        }
    }
}