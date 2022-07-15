using System;
using TMPro;
using UnityEngine;

namespace MOtter.Localization
{
    public class TextLocalizer : MonoBehaviour
    {
        [SerializeField] private string m_key;
        private TMP_Text m_textTarget;
        private Action<string, TextLocalizer> m_formatter;

        public string Key => m_key;
        public TMP_Text TextTarget => m_textTarget;
        public Action<string, TextLocalizer> Formatter => m_formatter;

        private void Start()
        {
            m_textTarget = GetComponent<TMP_Text>();
            MOtt.LANG.RegisterTextLocalizer(this);
        }


        public void SetKey(string key)
        {
            m_key = key;
            MOtt.LANG.ForceUpdate();
        }

        public void DeleteFormatters()
        {
            m_formatter = null;
        }

        public void SetFormatter(Action<string, TextLocalizer> formatter)
        {
            m_formatter += formatter;
            MOtt.LANG.ForceUpdate();
        }

        internal void UpdateComponent()
        {
            m_textTarget.ForceMeshUpdate();
        }


        private void OnDestroy()
        {
            MOtt.LANG.UnregisterTextLocalizer(this);
        }
    }
}