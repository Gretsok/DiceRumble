using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace MOtter.Localization
{
    [CustomEditor(typeof(LanguageData))]
    public class LanguageDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LanguageData targetLD = (LanguageData)target;
            GUILayout.Label("Number of entries : " + targetLD.LanguageDictionary.Count);
            foreach(KeyValuePair<string, string> kvp in targetLD.LanguageDictionary)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(kvp.Key.ToString());
                GUILayout.Label(kvp.Value.ToString());
                GUILayout.EndHorizontal();
            }
        }
    }
}
#endif