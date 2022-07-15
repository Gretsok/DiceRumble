using UnityEngine;

namespace DR.Gameplay.Dices
{
    [CreateAssetMenu(fileName = "DiceData", menuName = "Dice Rumble/Gameplay/Dices/DiceData")]
    public class DiceData : ScriptableObject
    {
        [SerializeField]
        private string m_diceName = "Dice";
        [SerializeField]
        private Sprite m_dicePreviewIcon = null;

        public string DiceName => m_diceName;
        public Sprite DicePreviewIcon => m_dicePreviewIcon;
    }
}