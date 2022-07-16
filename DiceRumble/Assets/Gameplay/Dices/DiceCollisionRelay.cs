using UnityEngine;

namespace DR.Gameplay.Dices
{
    public class DiceCollisionRelay : MonoBehaviour
    {
        [SerializeField]
        private Dice m_dice = null;
        public Dice Dice => m_dice;
    }
}