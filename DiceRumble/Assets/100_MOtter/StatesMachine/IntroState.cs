using System.Collections;
using System.Collections.Generic;
using DR.Gameplay.Level.Flow;
using MOtter;
using MOtter.StatesMachine;
using UnityEngine;

public class IntroState : FlowState
{
    public override void EnterState()
    {
        Debug.Log("EnterState : " + gameObject.name);
        MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().TurnManager.SetTeamTurn(Random.Range(0, 2));
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
