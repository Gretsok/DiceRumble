using DR.Gameplay.Level.Flow;
using MOtter;
using MOtter.StatesMachine;
using UnityEngine;

public class IntroState : FlowState
{
    [SerializeField]
    private LevelGameMode m_gamemode = null;
    [SerializeField]
    private float m_stateDuration = 2f;
    private float m_timeOfStart = float.MaxValue;
    private bool m_hasRequestedNextState = false;
    public override void EnterState()
    {
        Debug.Log("EnterState : " + gameObject.name);
        MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().TurnManager.SetTeamTurn(Random.Range(0, 2));
        m_hasRequestedNextState = false;
        m_timeOfStart = Time.time;
    }

    public override void UpdateState()
    {
        if(!m_hasRequestedNextState && Time.time - m_timeOfStart > m_stateDuration)
        {
            m_gamemode.SwitchToNextState();
        }
    }

}
