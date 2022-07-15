using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlexibleGrid : MonoBehaviour
{ 
    [SerializeField, Tooltip("")]
    private Vector2 m_preferedItemSize = default;
    [SerializeField, Tooltip("")]
    private Vector2 m_preferedItemSpacing = default;
    private RectTransform m_rectTransform = null;

    private void Start()
    {
        UpdateGrid();
    }

    public void UpdateGrid()
    {
        if(m_rectTransform == null)
        {
            m_rectTransform = transform as RectTransform;
        }

        // Getting every children
        List<RectTransform> l_childTransforms = new List<RectTransform>();
        for(int i = 0; i < transform.childCount; ++i)
        {
            l_childTransforms.Add(transform.GetChild(i) as RectTransform);
        }

        // Determining items possible per line and possible per column
        int l_itemsPerLine = (int) (m_rectTransform.rect.width / (m_preferedItemSize.x + m_preferedItemSpacing.x));
        int l_itemsPerColumn = (int) (m_rectTransform.rect.height / (m_preferedItemSize.y + m_preferedItemSpacing.y));

        // If the items doesn't fit, we calculate a reduction factor to make them fit
        float reductionFactor = 1f;
        if(l_childTransforms.Count > l_itemsPerColumn * l_itemsPerLine)
        {
            reductionFactor = Mathf.Sqrt((float) (l_itemsPerColumn * l_itemsPerLine) / (float) l_childTransforms.Count);
        }

        // Determining items to display per line and column with the reduction factor
        l_itemsPerLine = (int)(m_rectTransform.rect.width / ((m_preferedItemSize.x + m_preferedItemSpacing.x) * reductionFactor));
        l_itemsPerColumn = (int)(m_rectTransform.rect.height / ((m_preferedItemSize.y + m_preferedItemSpacing.y) * reductionFactor));

        int currXIndex = 0;
        int currYIndex = 0;
        // Calculating the border offset from top left corner 
        /*Vector2 borderOffset = Vector2.zero;
        borderOffset.x = (l_itemsPerLine * (m_preferedItemSize.x + m_preferedItemSpacing.x) * reductionFactor) - m_rectTransform.rect.width;
        borderOffset.y = (l_itemPerColumn * (m_preferedItemSize.y + m_preferedItemSpacing.y) * reductionFactor) - m_rectTransform.rect.height;
        borderOffset /= 2;
        */

        Vector2 offset = Vector2.zero;
        offset.x = m_preferedItemSize.x / 2f + (m_rectTransform.rect.width - l_itemsPerLine * m_preferedItemSize.x - (l_itemsPerLine - 1) * m_preferedItemSpacing.x) / 2f;
        offset.y = - m_preferedItemSize.y / 2f - (m_rectTransform.rect.height - l_itemsPerColumn * m_preferedItemSize.y - (l_itemsPerColumn - 1) * m_preferedItemSpacing.y) / 2f;

        // Applying size and position to children
        for (int i = 0; i < l_childTransforms.Count; ++i)
        {
            l_childTransforms[i].sizeDelta = m_preferedItemSize * reductionFactor;

            currXIndex = i % l_itemsPerLine;
            currYIndex = (int) (i / (l_itemsPerLine));

            l_childTransforms[i].localPosition = new Vector2(
                (m_preferedItemSize.x + ((currXIndex == 0) ? 0 : m_preferedItemSpacing.x)) * reductionFactor * currXIndex + offset.x/*+ m_preferedItemSize.x / 2 - borderOffset.x*/,
                -(m_preferedItemSize.y + ((currYIndex == 0) ? 0 : m_preferedItemSpacing.y)) * reductionFactor * currYIndex  + offset.y/*- m_preferedItemSize.y / 2 - borderOffset.y*/)
                - (new Vector2(m_rectTransform.rect.size.x, -m_rectTransform.rect.size.y) / 2) ;

            l_childTransforms[i].ForceUpdateRectTransforms();
        }
        Debug.Log($"Flexible grid updated with a reduction factor of {reductionFactor} and a border offset of {offset}");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FlexibleGrid))]
public class FlexibleGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Build Grid"))
        {
            FlexibleGrid gridTarget = (FlexibleGrid)target;
            gridTarget.UpdateGrid();
        }
    }
}
#endif