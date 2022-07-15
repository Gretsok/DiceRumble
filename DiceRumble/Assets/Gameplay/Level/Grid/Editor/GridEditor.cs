using UnityEditor;
using UnityEngine;

namespace DR.Gameplay.Level.Grid.Editor
{
    [CustomEditor(typeof(Grid))]
    public class GridEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(30f);

            if(GUILayout.Button("Generate Level"))
            {
                (target as Grid).GenerateLevel();
            }
        }
    }
}