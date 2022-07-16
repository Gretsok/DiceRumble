using System.Collections.Generic;
using DR.Gameplay.Dices;
using DR.Gameplay.Dices.Manager;
using DR.Gameplay.Level.Flow;
using MOtter.StatesMachine;
using UnityEngine;

public class FightState : FlowState
{
    [SerializeField]
    private LevelGameMode m_gamemode = null;
    private bool m_hasFinishedFighting = false;
    private bool m_askedForNextState = false;
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("EnterState : " + gameObject.name);
        m_askedForNextState = false;
        m_hasFinishedFighting = false;
        m_hasFinishedFighting = true;

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(m_hasFinishedFighting && !m_askedForNextState)
        {
            m_gamemode.SwitchToNextState();
            m_askedForNextState = true;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        DicesManager dm = m_gamemode.DicesManager;
        TurnManager turnManager = m_gamemode.TurnManager;
        int turnTeam = turnManager.TurnTeam;
        List<Dice> teamDices = turnTeam == 0 ? dm.FirstTeamDices : dm.SecondTeamDices;
        foreach (Dice dice in teamDices)
        {
            dice.EndTurnUpdates();
        }
        turnManager.ChangeTurn();
        Debug.Log("ExitState : " + gameObject.name);
    }
}
