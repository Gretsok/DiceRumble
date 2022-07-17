using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusIconsManager : MonoBehaviour
{
    public enum Status
    {
        Root,
        Fire,
        Plant,
        Shield,
        Poison
    }
    
    [SerializeField] private RectTransform m_rootIcon;
    [SerializeField] private RectTransform m_fireIcon;
    [SerializeField] private RectTransform m_plantIcon;
    [SerializeField] private RectTransform m_shieldIcon;
    [SerializeField] private RectTransform m_poisonIcon;
    
    [SerializeField] private TextMeshProUGUI m_rootCounter;
    [SerializeField] private TextMeshProUGUI m_fireCounter;
    [SerializeField] private TextMeshProUGUI m_plantCounter;
    [SerializeField] private TextMeshProUGUI m_shieldCounter;
    [SerializeField] private TextMeshProUGUI m_poisonCounter;

    private Dictionary<Status, RectTransform> m_icons = new ();
    private Dictionary<Status, TextMeshProUGUI> m_counters = new ();
    private List<RectTransform> m_activeStatus = new ();

    private void Start()
    {
        //icons
        m_icons[Status.Root] = m_rootIcon;
        m_icons[Status.Fire] = m_fireIcon;
        m_icons[Status.Plant] = m_plantIcon;
        m_icons[Status.Shield] = m_shieldIcon;
        m_icons[Status.Poison] = m_poisonIcon;

        //counters
        m_counters[Status.Root] = m_rootCounter;
        m_counters[Status.Fire] = m_fireCounter;
        m_counters[Status.Plant] = m_plantCounter;
        m_counters[Status.Shield] = m_shieldCounter;
        m_counters[Status.Poison] = m_poisonCounter;
        
        foreach (Transform icon in transform)
        {
            icon.gameObject.SetActive(false);
        }
    }

    public void AddStatusStack(Status p_status, int p_amount)
    {
        RectTransform status = m_icons[p_status];
        TextMeshProUGUI counter = m_counters[p_status];
        int currentCount = 0;
        if (!m_activeStatus.Contains(status))
        {
            m_activeStatus.Add(status);
            status.gameObject.SetActive(true);
        }
        else
        {
            currentCount = int.Parse(counter.text);
        }
        currentCount += p_amount;
        counter.text = currentCount.ToString();
        UpdateDisplay();
    }
    
    public void RemoveStatusStack(Status p_status, int p_amount, bool p_removeAll)
    {
        RectTransform status = m_icons[p_status];
        TextMeshProUGUI counter = m_counters[p_status];
        if (!m_activeStatus.Contains(status))
            return;
        if (!p_removeAll && int.Parse(counter.text) > p_amount)
        {
            counter.text = (int.Parse(counter.text) - p_amount).ToString();
        }
        else
        {
            m_activeStatus.Remove(status);  
            status.gameObject.SetActive(false);
            counter.text = "0";
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        int i = 0;
        foreach (RectTransform icon in m_activeStatus)
        {
            Vector3 pos = icon.localPosition;
            pos.x = i * icon.rect.width;
            icon.localPosition = pos;
            i++;
        }
    }
}
