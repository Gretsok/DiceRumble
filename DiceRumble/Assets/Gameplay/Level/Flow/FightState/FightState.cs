using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using DR.Gameplay.Dices;
using DR.Gameplay.Dices.Manager;
using DR.Gameplay.Level.Flow;
using DR.Gameplay.Level.Grid;
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
            List<Dice> teamDices = m_gamemode.TurnManager.TurnTeam == 0 ? m_gamemode.DicesManager.FirstTeamDices : m_gamemode.DicesManager.SecondTeamDices;
            List<Dice> opponentDices = m_gamemode.TurnManager.TurnTeam == 1 ? m_gamemode.DicesManager.FirstTeamDices : m_gamemode.DicesManager.SecondTeamDices;
            //Active powers
            foreach (Dice dice in teamDices)
            {
                if (dice.CombatController.GetDamages() == 6)
                {
                    dice.CombatController.UsePower();
                }
            }
            //Fight dices
            foreach (Dice dice in teamDices)
            {
                List<Tile> surroundingTiles = m_gamemode.Grid.GetSurroundingTiles(dice.DiceMovementController.GamePosition);
                foreach (Tile tile in surroundingTiles)
                {
                    if (tile.CurrentDice is not null && tile.CurrentDice.TeamIndex != dice.TeamIndex)
                    {
                        Debug.Log(dice.gameObject.name + " attacks " + tile.CurrentDice.gameObject.name);
                        dice.CombatController.Attack(tile.CurrentDice.CombatController);
                        yield return new WaitForSeconds(0.7f);
                    }
                }
            }

            //Check dead dices
            for(int i = teamDices.Count - 1; i >= 0; --i)
            {
                if (teamDices[i].CombatController.CurrentHealth <= 0)
                    teamDices[i].CombatController.Die();
            }
            for (int i = opponentDices.Count - 1; i >= 0; --i)
            {
                if (opponentDices[i].CombatController.CurrentHealth <= 0)
                    opponentDices[i].CombatController.Die();
            }
            
            //finish fight
            m_hasFinishedFighting = true;
        }
    }
}