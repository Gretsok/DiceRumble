using UnityEngine;

namespace DR.Gameplay.Dices.Editor
{
    [UnityEditor.CustomEditor(typeof(Dice))]
    public class DiceTestEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(30f);

            if (GUILayout.Button("Roll forward"))
            {

            }
            if (GUILayout.Button("Roll backward"))
            {

            }
            if (GUILayout.Button("Roll rightward"))
            {

            }
            if (GUILayout.Button("Roll leftward"))
            {

            }
        }
    }
}