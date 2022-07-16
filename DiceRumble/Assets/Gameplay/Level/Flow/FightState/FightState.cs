using System.Collections;
using System.Collections.Generic;
using DR.Gameplay.Dices;
using DR.Gameplay.Dices.Manager;
using DR.Gameplay.Level.Flow;
using MOtter;
using MOtter.StatesMachine;
using UnityEngine;

public class FightState : FlowState
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
        DicesManager dm = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().DicesManager;
        TurnManager turnManager = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().TurnManager;
        int teamTurn = turnManager.TeamTurn;
        List<Dice> teamDices = teamTurn == 0 ? dm.FirstTeamDices : dm.SecondTeamDices;
        foreach (Dice dice in teamDices)
        {
            dice.EndTurnUpdates();
        }
        turnManager.ChangeTurn();
        Debug.Log("ExitState : " + gameObject.name);
    }
}
