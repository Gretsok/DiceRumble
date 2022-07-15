
namespace MOtter.StatesMachine
{
    public class StateMachine
    {
        protected State m_currentState = null;
        
        public bool IsRunning { private set; get; }

        internal virtual void EnterStateMachine(State a_entryState)
        {
            IsRunning = true;
            SwitchToState(a_entryState);
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

        public virtual void SwitchToState(State a_state)
        {

            if (m_currentState != null)
            {
                if (a_state != null)
                {
                    a_state.PreviousState = m_currentState;
                }
                m_currentState?.ExitState();
            }

            m_currentState = a_state;
            m_currentState?.EnterState();
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

        public State GetCurrentState()
        {
            return m_currentState;
        }

        public T GetCurrentState<T>() where T : State
        {
            return (T)m_currentState;
        }
    }
}