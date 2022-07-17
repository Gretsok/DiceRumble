using System.Collections;
using System.Collections.Generic;
using DR.Gameplay.Dices;
using DR.Gameplay.Dices.Manager;
using DR.Gameplay.Level.Grid;
using MOtter.SoundManagement;
using MOtter.StatesMachine;
using UnityEngine;

namespace DR.Gameplay.Level.Flow.FightState
{
    public class FightState : FlowState
    {
        [SerializeField]
        private LevelGameMode m_gamemode = null;
        private bool m_hasFinishedFighting = false;
        private bool m_askedForNextState = false;

        [SerializeField]
        private SoundData m_punchSoundData = null;
        [SerializeField]
        private SoundData m_deathSoundData = null;

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("EnterState : " + gameObject.name);
            m_askedForNextState = false;
            m_hasFinishedFighting = false;
            StartCoroutine(Fight());
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (m_hasFinishedFighting && !m_askedForNextState)
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

        private IEnumerator Fight()
        {
            //Fight dices
            List<Dice> teamDices = m_gamemode.TurnManager.TurnTeam == 0 ? m_gamemode.DicesManager.FirstTeamDices : m_gamemode.DicesManager.SecondTeamDices;
            List<Dice> opponentDices = m_gamemode.TurnManager.TurnTeam == 1 ? m_gamemode.DicesManager.FirstTeamDices : m_gamemode.DicesManager.SecondTeamDices;
            foreach (Dice dice in teamDices)
            {
                List<Tile> surroundingTiles = m_gamemode.Grid.GetSurroundingTiles(dice.DiceMovementController.GamePosition);
                foreach (Tile tile in surroundingTiles)
                {
                    if (tile.CurrentDice is not null && tile.CurrentDice.TeamIndex != dice.TeamIndex)
                    {
                        yield return new WaitForSeconds(0.5f);
                        Debug.Log(dice.gameObject.name + " attacks " + tile.CurrentDice.gameObject.name);
                        dice.CombatController.Attack(tile.CurrentDice.CombatController);
                        MOtter.MOtt.SOUND.Play2DSound(m_punchSoundData);
                        yield return new WaitForSeconds(0.5f);
                    }
                }
            }

            //Check dead dices
            for(int i = teamDices.Count - 1; i >= 0; --i)
            {
                if (teamDices[i].CombatController.CurrentHealth <= 0)
                {
                    teamDices[i].CombatController.Die();
                    MOtter.MOtt.SOUND.Play2DSound(m_deathSoundData);
                }
            }
            for (int i = opponentDices.Count - 1; i >= 0; --i)
            {
                if (opponentDices[i].CombatController.CurrentHealth <= 0)
                {
                    opponentDices[i].CombatController.Die();
                    MOtter.MOtt.SOUND.Play2DSound(m_deathSoundData);
                }
            }


            //finish fight
            m_hasFinishedFighting = true;
        }
    }
}