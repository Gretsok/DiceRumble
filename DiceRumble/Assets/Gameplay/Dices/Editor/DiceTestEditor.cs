using UnityEngine;

namespace DR.Gameplay.Dices.Editor
{
    [UnityEditor.CustomEditor(typeof(DiceMovementController))]
    public class DiceTestEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(30f);

            if (GUILayout.Button("Roll forward"))
            {
                (target as DiceMovementController).RollForward();
            }
            if (GUILayout.Button("Roll backward"))
            {
                (target as DiceMovementController).RollBackward();
            }
            if (GUILayout.Button("Roll rightward"))
            {
                (target as DiceMovementController).RollRightward();
            }
            if (GUILayout.Button("Roll leftward"))
            {
                (target as DiceMovementController).RollLeftward();
            }
        }
    }
}