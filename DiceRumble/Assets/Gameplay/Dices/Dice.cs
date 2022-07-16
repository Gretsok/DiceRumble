﻿using UnityEngine;

namespace DR.Gameplay.Dices
{
    public class Dice : MonoBehaviour
    {
        public enum EDiceTopFace
        {
            ONE,
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX
        }
        #region Dice Rotations for Faces
        private Quaternion FACE_ONE = Quaternion.identity;
        private Quaternion FACE_TWO = Quaternion.Euler(0f, 0f, 90f);
        private Quaternion FACE_THREE = Quaternion.Euler(90f, 0f, 0f);
        private Quaternion FACE_FOUR = Quaternion.Euler(0f, 0f, 90f);
        private Quaternion FACE_FIVE = Quaternion.Euler(-90f, 0f, 0f);
        private Quaternion FACE_SIX = Quaternion.Euler(180f, 0f, 0f);
        #endregion

        private EDiceTopFace m_diceTopFace = default;
        public EDiceTopFace DiceTopFace => m_diceTopFace;

        [SerializeField]
        private Animations.DiceAnimationsHandler m_animationsHandler = null;

        public void SetTopFace(EDiceTopFace a_diceTopFace)
        {
            m_diceTopFace = a_diceTopFace;
        }

        public void RollForward()
        {
            m_animationsHandler.TriggerRollForwardAnimations();
        }

        public void RollBackward()
        {
            m_animationsHandler.TriggerRollBackwardAnimations();
        }

        public void RollLeftward()
        {
            m_animationsHandler.TriggerRollLeftwardAnimations();
        }

        public void RollRightward()
        {
            m_animationsHandler.TriggerRollRightwardAnimations();
        }

    }
}