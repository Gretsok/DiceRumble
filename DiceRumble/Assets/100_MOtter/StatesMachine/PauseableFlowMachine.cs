using System;
using UnityEngine;

namespace MOtter.StatesMachine
{
    public class PauseableFlowMachine : MainFlowMachine
    {
        [SerializeField] private StateMachineMonoBehaviour m_pauseFlowMachine = null;
        private bool m_isPaused = false;
        [SerializeField] private bool m_stopTimeInPause = true;
        [SerializeField] private bool m_hidePanelsInPause = true;
        public bool IsPaused => m_isPaused;

        public Action OnPause = null;
        public Action OnUnpause = null;

        public override void DoUpdate()
        {
            if (m_isPaused)
            {
                m_pauseFlowMachine?.DoUpdate();
            }
            else
            {
                m_currentState?.UpdateState();
            }
        }
        public override void DoFixedUpdate()
        {
            if (m_isPaused)
            {
                m_pauseFlowMachine?.DoFixedUpdate();
            }
            else
            {
                m_currentState?.FixedUpdateState();
            }

        }

        public override void DoLateUpdate()
        {
            if (m_isPaused)
            {
                m_pauseFlowMachine?.DoLateUpdate();
            }
            else
            {
                m_currentState?.LateUpdateState();
            }

        }

        public virtual void Pause()
        {
            if(!m_isPaused)
            {
                m_isPaused = true;
                if (m_stopTimeInPause)
                    Time.timeScale = 0;
                OnPause?.Invoke();
                if (m_currentState != null && m_currentState is FlowState)
                {
                    (m_currentState as FlowState).UnregisterEvents();
                    if (m_hidePanelsInPause)
                    {
                        (m_currentState as FlowState).HidePanels();
                    }
                }
                m_pauseFlowMachine.EnterStateMachine();
            }
        }

        public virtual void Unpause()
        {
            if(m_isPaused)
            {
                m_isPaused = false;
                if (m_stopTimeInPause)
                    Time.timeScale = 1;
                m_pauseFlowMachine.ExitStateMachine();
                if (m_currentState != null && m_currentState is FlowState)
                {
                    if (m_hidePanelsInPause)
                    {
                        (m_currentState as FlowState).DisplayPanels();
                    }
                    (m_currentState as FlowState).RegisterEvents();
                }
                OnUnpause?.Invoke();
            }
            
        }
    }
}
