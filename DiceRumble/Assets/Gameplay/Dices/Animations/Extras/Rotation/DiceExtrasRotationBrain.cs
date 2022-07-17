using System.Collections;
using UnityEngine;

namespace DR.Gameplay.Dices.Animations.Extras.Rotation
{
    public class DiceExtrasRotationBrain : MonoBehaviour
    {
        private Level.Grid.Grid m_grid = null;
        private Coroutine m_routine = null;
        public void SetUp(Level.Grid.Grid a_grid)
        {
            m_grid = a_grid;
        }

        public void LookForward()
        {
            if (m_routine != null)
            {
                StopCoroutine(m_routine);
            }

            StartCoroutine(ChangingLookRoutine(m_grid.transform.forward));
        }

        public void LookBackward()
        {
            if (m_routine != null)
            {
                StopCoroutine(m_routine);
            }

            StartCoroutine(ChangingLookRoutine(-m_grid.transform.forward));
        }

        public void LookLeftward()
        {
            if (m_routine != null)
            {
                StopCoroutine(m_routine);
            }

            StartCoroutine(ChangingLookRoutine(-m_grid.transform.right));
        }

        public void LookRightward()
        {
            if (m_routine != null)
            {
                StopCoroutine(m_routine);
            }
            StartCoroutine(ChangingLookRoutine(m_grid.transform.right));
        }

        private IEnumerator ChangingLookRoutine(Vector3 a_targettedForward)
        {
            Quaternion startingRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            Quaternion targettedRotation = Quaternion.LookRotation(a_targettedForward, Vector3.up);
            float durationToChange = 0.5f;
            float timeOfStart = Time.time;
            while(Time.time - timeOfStart < durationToChange)
            {
                transform.rotation = Quaternion.Slerp(startingRotation, targettedRotation, (Time.time - timeOfStart) / durationToChange);
                yield return null;
            }
            transform.forward = a_targettedForward;
        }
    }
}