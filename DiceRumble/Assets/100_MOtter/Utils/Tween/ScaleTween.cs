using UnityEngine;

namespace Tween
{
    [System.Serializable]
    public class ScaleTween : ATween
    {
        [SerializeField]
        private Vector3 m_initialLocalScale = Vector3.one;
        [SerializeField]
        private Vector3 m_finalLocalScale = Vector3.one;

        protected override void SetStartingValues()
        {
            base.SetStartingValues();
            m_target.localScale = m_initialLocalScale;
        }

        protected override void SetFinalValues()
        {
            m_target.localScale = m_finalLocalScale;
            base.SetFinalValues();
        }

        protected override void ManageTween(float interpolationValue)
        {
            base.ManageTween(interpolationValue);
            m_target.localScale = Vector3.Lerp(m_initialLocalScale, m_finalLocalScale, interpolationValue);
        }
    }
}