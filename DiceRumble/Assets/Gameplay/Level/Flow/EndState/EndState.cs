using MOtter.LevelData;
using MOtter.SoundManagement;
using MOtter.StatesMachine;
using UnityEngine;

namespace DR.Gameplay.Level.Flow.EndState
{
    public class EndState : FlowState
    {
        [SerializeField]
        private LevelGameMode m_gamemode = null;
        private EndPanel m_panel = null;
        [SerializeField]
        private LevelData m_levelData = null;
        [SerializeField]
        private float m_stateDuration = 6f;
        private float m_timeOfStart = float.MaxValue;
        private bool m_hasStartedLoadingMenu = false;
        [SerializeField]
        private SoundData m_endJingle = null;

        internal override void RegisterReferences()
        {
            base.RegisterReferences();
            m_panel = GetPanel<EndPanel>();
        }

        internal override void SetUpDependencies()
        {
            base.SetUpDependencies();
            if(m_gamemode.DicesManager.FirstTeamDices.Count == 0)
            {
                m_panel.InflateWinningTeam(1);
            }
            else
            {
                m_panel.InflateWinningTeam(0);
            }
            m_hasStartedLoadingMenu = false;
        }

        public override void EnterState()
        {
            base.EnterState();
            m_timeOfStart = Time.time;
            m_gamemode.StopMusics();
            MOtter.MOtt.SOUND.Play2DSound(m_endJingle);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if(!m_hasStartedLoadingMenu && Time.time - m_timeOfStart > m_stateDuration)
            {
                m_levelData.LoadLevel();
                m_hasStartedLoadingMenu = true;
            }
        }
    }
}