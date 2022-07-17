using MOtter.SoundManagement;
using MOtter.StatesMachine;
using System.Collections;
using UnityEngine;

namespace DR.Gameplay.Level.Flow
{
    public class LevelGameMode : MainFlowMachine
    {
        [SerializeField]
        private Dices.Manager.DicesManager m_dicesManager = null;
        public Dices.Manager.DicesManager DicesManager => m_dicesManager;
        
        [SerializeField] 
        private Gameplay.Level.Flow.TurnManager m_turnManager = null;
        public Gameplay.Level.Flow.TurnManager TurnManager => m_turnManager;

        [SerializeField] 
        private Gameplay.Data.GameplayData m_gameplayData = null;
        public Gameplay.Data.GameplayData GameplayData => m_gameplayData;

        private Grid.Grid m_grid = null;
        public Grid.Grid Grid => m_grid;
        private Teams.TeamsDataConveyor m_teamsDataConveyor = null;

        [SerializeField]
        private FlowState m_endState = null;

        [SerializeField]
        private SoundData m_musicData = null;
        private AudioSource m_musicSource = null;

        public override IEnumerator LoadAsync()
        {
            while(m_grid == null)
            {
                m_grid = FindObjectOfType<Grid.Grid>();
                yield return null;
            }

            m_teamsDataConveyor = MOtter.MOtt.DATACONVEY.GetFirstContainer<Teams.TeamsDataConveyor>();
            MOtter.MOtt.DATACONVEY.UnregisterContainer(m_teamsDataConveyor);

            m_dicesManager.SetUpDices(m_teamsDataConveyor, m_grid);
            m_dicesManager.OnTeamEliminated += HandleTeamEliminated;

            yield return StartCoroutine(base.LoadAsync());
        }

        internal override void EnterStateMachine()
        {
            base.EnterStateMachine();
            m_musicSource = MOtter.MOtt.SOUND.Play2DSound(m_musicData, true);
        }

        internal override void ExitStateMachine()
        {
            base.ExitStateMachine();
            StopMusics();
        }

        public void StopMusics()
        {
            if (m_musicSource != null)
            {
                m_musicSource.Stop();
                m_musicSource = null;
            }
        }

        private void HandleTeamEliminated()
        {
            m_dicesManager.OnTeamEliminated -= HandleTeamEliminated;

            SwitchToState(m_endState);
        }
    }
}