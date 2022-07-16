using UnityEngine;

namespace DR.Gameplay.Dices.Animations
{
    public class DiceAnimationsHandler : MonoBehaviour
    {
        private int ROLL_FORWARD = Animator.StringToHash("RollForward");
        private int ROLL_BACKWARD = Animator.StringToHash("RollBackward");
        private int ROLL_LEFTWARD = Animator.StringToHash("RollLeftward");
        private int ROLL_RIGHTWARD = Animator.StringToHash("RollRightward");

        [SerializeField]
        private Animator m_movementAnimator = null;
        [SerializeField]
        private Animator m_rotationAnimator = null;

        public void TriggerRollForwardAnimations()
        {
            m_movementAnimator.SetTrigger(ROLL_FORWARD);
            m_rotationAnimator.SetTrigger(ROLL_FORWARD);
        }

        public void TriggerRollBackwardAnimations()
        {
            m_movementAnimator.SetTrigger(ROLL_BACKWARD);
            m_rotationAnimator.SetTrigger(ROLL_BACKWARD);
        }

        public void TriggerRollLeftwardAnimations()
        {
            m_movementAnimator.SetTrigger(ROLL_LEFTWARD);
            m_rotationAnimator.SetTrigger(ROLL_LEFTWARD);
        }

        public void TriggerRollRightwardAnimations()
        {
            m_movementAnimator.SetTrigger(ROLL_RIGHTWARD);
            m_rotationAnimator.SetTrigger(ROLL_RIGHTWARD);
        }
    }
}