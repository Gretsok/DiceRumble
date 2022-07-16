using UnityEngine;

namespace DR.Gameplay.Dices
{
    public class DiceFaceHandler : MonoBehaviour
    {
        [SerializeField]
        private DiceMovementController.EDiceTopFace m_diceTopFaceIfThisFaceIsOnGround = default;
        public DiceMovementController.EDiceTopFace DiceTopFaceIfThisFaceIsOnGround => m_diceTopFaceIfThisFaceIsOnGround;
    }
}