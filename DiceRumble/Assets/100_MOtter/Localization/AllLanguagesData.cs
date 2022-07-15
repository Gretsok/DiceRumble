using System.Collections.Generic;
using UnityEngine;

namespace MOtter.Localization
{
    [CreateAssetMenu(fileName = "AllLanguagesData", menuName = "Localization/AllLanguagesData")]
    public class AllLanguagesData : ScriptableObject
    {
        [SerializeField]
        private List<LanguageData> m_languagesData = new List<LanguageData>();
        public int NumberOfLanguages => m_languagesData.Count;
        public void AddOrModifiyLanguageData(LanguageData languageData, int index)
        {
            if (index >= m_languagesData.Count)
            {
                for (int i = m_languagesData.Count; i <= index; i++)
                {
                    Debug.Log("Adding a new language to the system");
                    m_languagesData.Add(null);
                }
            }

            m_languagesData[index] = languageData;
        }

        public LanguageData GetLanguageData(int index)
        {
            if (index >= 0 && index < m_languagesData.Count)
            {
                return m_languagesData[index];
            }
            Debug.LogError("Invalid Index when getting Language Data");
            return null;
        }
    }
}