using UnityEngine;

namespace Tween
{
    [System.Serializable]
    public class RotationTween : ATween
    {
        [SerializeField]
        private Vector3 m_initialLocalRotation = Vector3.zero;
        [SerializeField]
        private Vector3 m_finalLocalRotation = Vector3.zero;

        protected override void SetStartingValues()
        {
            base.SetStartingValues();
            m_target.localEulerAngles = m_initialLocalRotation;
        }

        protected override void SetFinalValues()
        {
            m_target.localEulerAngles = m_finalLocalRotation;
            base.SetFinalValues();
        }

        protected override void ManageTween(float interpolationValue)
        {
            base.ManageTween(interpolationValue);
            m_target.localEulerAngles = Vector3.Lerp(m_initialLocalRotation, m_finalLocalRotation, interpolationValue);
        }
    }
}