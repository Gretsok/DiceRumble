using MOtter.Localization;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MOtter.Localization
{
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if (GUILayout.Button("Generate Localization Data"))
            {
                ((LocalizationManager)target).GenerateLocalizationData();
            }
        }
    }
}
#endif