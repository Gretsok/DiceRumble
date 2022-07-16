using System.Collections;
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
        private Transform m_rotationModelTransform = null;

        public void TriggerRollForwardAnimations()
        {
            m_movementAnimator.SetTrigger(ROLL_FORWARD);
            StartCoroutine(RotateRoutine(transform.right * 90f));
        }

        public void TriggerRollBackwardAnimations()
        {
            m_movementAnimator.SetTrigger(ROLL_BACKWARD);
            StartCoroutine(RotateRoutine(transform.right * -90f));
        }

        public void TriggerRollLeftwardAnimations()
        {
            m_movementAnimator.SetTrigger(ROLL_LEFTWARD);
            StartCoroutine(RotateRoutine(transform.forward * 90f));
        }

        public void TriggerRollRightwardAnimations()
        {
            m_movementAnimator.SetTrigger(ROLL_RIGHTWARD);
            StartCoroutine(RotateRoutine(transform.forward * -90f));
        }

        private IEnumerator RotateRoutine(Vector3 a_rotationToAdd, float a_timeToRotate = 0.67f)
        {
            float timeOfStart = Time.time;
            Quaternion startingRotation = m_rotationModelTransform.rotation;
            m_rotationModelTransform.Rotate(a_rotationToAdd, Space.World);
            Quaternion finalRotation = m_rotationModelTransform.rotation;
            m_rotationModelTransform.rotation = startingRotation;
            while(Time.time - timeOfStart <= a_timeToRotate)
            {
                Debug.Log($"TIME : {Time.time - timeOfStart}");
                m_rotationModelTransform.rotation = Quaternion.Slerp(startingRotation, finalRotation, (Time.time - timeOfStart) / a_timeToRotate);
                yield return null;
            }
            Debug.Log("Rotation finish");
            Debug.Log($"TIME : {Time.time - timeOfStart}");
            m_rotationModelTransform.rotation = finalRotation;
        }
    }
}