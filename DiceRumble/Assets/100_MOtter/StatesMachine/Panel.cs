using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    private RectTransform m_rectTransform = null;
    public RectTransform RectTransform => m_rectTransform;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    /// <summary>
    /// Display Panel
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide Panel
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
