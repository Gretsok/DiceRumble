
namespace MOtter.StatesMachine
{
    public class State
    {
        public State PreviousState { get; set; }

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