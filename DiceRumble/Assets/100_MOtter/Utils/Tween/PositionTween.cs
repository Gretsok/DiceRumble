using UnityEngine;

namespace Tween
{
    [System.Serializable]
    public class PositionTween : ATween
    {
        [SerializeField]
        private Vector3 m_initialLocalPosition = Vector3.one;
        [SerializeField]
        private Vector3 m_finalLocalPosition = Vector3.one;

        protected override void SetStartingValues()
        {
            base.SetStartingValues();
            m_target.localPosition = m_initialLocalPosition;
        }

        protected override void SetFinalValues()
        {
            m_target.localPosition = m_finalLocalPosition;
            base.SetFinalValues();
        }

        protected override void ManageTween(float interpolationValue)
        {
            base.ManageTween(interpolationValue);
            m_target.localPosition = Vector3.Lerp(m_initialLocalPosition, m_finalLocalPosition, interpolationValue);
        }
    }
}