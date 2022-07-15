using System.Collections.Generic;
using UnityEngine;

namespace MOtter.Localization
{
    public class LanguageData : ScriptableObject
    {
        [SerializeField]
        private LanguageDictionary m_languageDictionary = new LanguageDictionary();
        internal LanguageDictionary LanguageDictionary => m_languageDictionary;

        internal void SetLanguageDictionary(LanguageDictionary languageDictionary)
        {
            m_languageDictionary = languageDictionary;
        }
    }
}