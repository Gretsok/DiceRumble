using UnityEngine;

namespace MOtter.StatesMachine
{
    public class StateMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private StateMonoBehaviour m_nextState = null;

        public StateMonoBehaviour NextState => m_nextState;
        public StateMonoBehaviour PreviousState { get; set; }

        public virtual void EnterState()
        {
            //Debug.Log("EnterState : " + gameObject.name);
        }

        public virtual void UpdateState()
        {

        }

        public virtual void FixedUpdateState()
        {

        }

        public virtual void LateUpdateState()
        {

        }

        public virtual void ExitState()
        {
            //Debug.Log("ExitState : " + gameObject.name);
        }
    }
}