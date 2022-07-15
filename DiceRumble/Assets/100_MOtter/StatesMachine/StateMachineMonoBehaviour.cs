using System;
using System.Collections;
using UnityEngine;


namespace MOtter.StatesMachine
{
    public class StateMachineMonoBehaviour : MonoBehaviour
    {
        [SerializeField] protected StateMonoBehaviour m_defaultState = null;
        protected StateMonoBehaviour m_currentState = null;
        public bool IsRunning { private set; get; }

        internal virtual void EnterStateMachine()
        {
            IsRunning = true;
            SwitchToState(m_defaultState);
        }

        public virtual void DoUpdate()
        {
            m_currentState?.UpdateState();
        }

        public virtual void DoFixedUpdate()
        {
            m_currentState?.FixedUpdateState();
        }

        public virtual void DoLateUpdate()
        {
            m_currentState?.LateUpdateState();
        }

        internal virtual void ExitStateMachine()
        {
            m_currentState?.ExitState();
            IsRunning = false;
        }

        public virtual void SwitchToState(StateMonoBehaviour state)
        {

            if (m_currentState != null)
            {
                if(state != null)
                {
                    state.PreviousState = m_currentState;
                }
                m_currentState?.ExitState();
            }

            m_currentState = state;
            m_currentState?.EnterState();
        }

        public virtual void SwitchToNextState()
        {
            SwitchToState(m_currentState.NextState);
        }

        public virtual void SwitchToPreviousState()
        {
            if (m_currentState != null)
            {
                m_currentState?.ExitState();
            }
            m_currentState = m_currentState.PreviousState;
            m_currentState?.EnterState();
        }



        public StateMonoBehaviour GetCurrentState()
        {
            return m_currentState;
        }

        public T GetCurrentState<T>() where T : StateMonoBehaviour
        {
            return (T)m_currentState;
        }

    }
}