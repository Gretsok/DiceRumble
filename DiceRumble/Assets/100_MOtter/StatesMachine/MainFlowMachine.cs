using System;
using System.Collections;

namespace MOtter.StatesMachine
{
    public class MainFlowMachine : StateMachineMonoBehaviour
    {
        private bool m_isLoaded = false;
        private bool m_isUnloaded = false;

        public bool IsLoaded => m_isLoaded;
        public bool IsUnloaded => m_isUnloaded;

        public LevelData.LevelData LevelData { get; internal set; }

        private void Awake()
        {
            MOtt.GM.RegisterFlowMachine(this);
        }

        public virtual IEnumerator LoadAsync()
        {
            yield return null;
            m_isLoaded = true;
            EnterStateMachine();
        }

        public virtual IEnumerator UnloadAsync(Action onUnloadEnded)
        {

            yield return null;
            m_isUnloaded = true;
            yield return null;
            ExitStateMachine();
            yield return null;
            if (onUnloadEnded != null) onUnloadEnded();
        }

    }
}