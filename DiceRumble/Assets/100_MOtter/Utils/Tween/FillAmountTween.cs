using System.Collections;
using System.Collections.Generic;
using Tween;
using UnityEngine;
using UnityEngine.UI;

namespace Tween
{
    [System.Serializable, RequireComponent(typeof(Image))]
    public class FillAmountTween : ATween
    {
        [SerializeField]
        private float m_initialValue = 0f;
        [SerializeField]
        private float m_finalValue = 0f;

        private Image m_imageTarget = null;

        protected override void Start()
        {
            base.Start();
            m_imageTarget = m_target.GetComponent<Image>();
        }

        protected override void SetFinalValues()
        {
            base.SetFinalValues();
            m_imageTarget.fillAmount = m_initialValue;
        }

        protected override void SetStartingValues()
        {
            base.SetStartingValues();
            m_imageTarget.fillAmount = m_finalValue;
        }

        protected override void ManageTween(float interpolationValue)
        {
            base.ManageTween(interpolationValue);
            m_imageTarget.fillAmount = Mathf.Lerp(m_initialValue, m_finalValue, interpolationValue);
        }
    }
}