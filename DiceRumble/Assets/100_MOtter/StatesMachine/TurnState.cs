using System.Collections;
using System.Collections.Generic;
using MOtter.StatesMachine;
using UnityEngine;

public class TurnState : FlowState
{
    public override void EnterState()
    {
        Debug.Log("EnterState : " + gameObject.name);
        
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {

    }

    public override void LateUpdateState()
    {

    }

    public override void ExitState()
    {
        Debug.Log("ExitState : " + gameObject.name);
    }
}
