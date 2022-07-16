using System.Collections;
using MOtter.StatesMachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DR.Gameplay.Level.Flow
{
    public class TurnState : FlowState
    {
        [SerializeField]
        private LevelGameMode m_gamemode = null;
        [SerializeField]
        private int m_numberMaxOfMoves = 5;
        private int m_movesDoneThisTurn = 0;
        private Dices.Dice m_currentSelectedDice = null;
        private List<Grid.Tile> m_surroundingTiles = null;
        private bool m_diceIsMoveing;
        
        internal override void RegisterEvents()
        {
            base.RegisterEvents();
            MOtter.MOtt.PLAYERS.GetActions(0).FindActionMap("Gameplay").FindAction("Select").started += HandleSelectStarted;
        }

        internal override void UnregisterEvents()
        {
            base.UnregisterEvents();
            MOtter.MOtt.PLAYERS.GetActions(0).FindActionMap("Gameplay").FindAction("Select").started -= HandleSelectStarted;
        }

        public override void EnterState()
        {
            base.EnterState();
            m_movesDoneThisTurn = 0;
        }

        private void HandleSelectStarted(InputAction.CallbackContext obj)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit l_hitInfo))
            {
                if(l_hitInfo.collider.TryGetComponent(out Dices.DiceCollisionRelay l_dice)
                    && ((m_gamemode.TurnManager.TurnTeam == 0 && m_gamemode.DicesManager.FirstTeamDices.Contains(l_dice.Dice)) 
                        || (m_gamemode.TurnManager.TurnTeam == 1 && m_gamemode.DicesManager.SecondTeamDices.Contains(l_dice.Dice))))
                {
                    m_currentSelectedDice = l_dice.Dice;
                    EmptySurroundingTiles();
                    GetSurroundingTiles();
                }
                else if(l_hitInfo.collider.TryGetComponent(out Grid.Tile l_tile) && m_currentSelectedDice != null)
                {
                    if(m_surroundingTiles.Contains(l_tile))
                    {
                        if(m_gamemode.Grid.TryToGetTile(l_tile.GamePosition) != null
                            && l_tile.TryToSetCurrentDice(m_currentSelectedDice))
                        {
                            MoveDiceToTile(l_tile);
                            m_currentSelectedDice = null;
                            EmptySurroundingTiles();
                        }
                        
                    }
                }
            }
        }

        private void MoveDiceToTile(Grid.Tile a_tile)
        {
            var moveController = m_currentSelectedDice.GetComponent<Dices.DiceMovementController>();
            if (moveController.GamePosition.x > a_tile.GamePosition.x)
            {
                moveController.RollLeftward();
            }
            else if(moveController.GamePosition.x < a_tile.GamePosition.x)
            {
                moveController.RollRightward();
            }
            else if(moveController.GamePosition.y > a_tile.GamePosition.y)
            {
                moveController.RollForward();
            }
            else if(moveController.GamePosition.y < a_tile.GamePosition.y)
            {
                moveController.RollBackward();
            }
            else
            {
                Debug.LogError("What the heck");
            }
            m_movesDoneThisTurn++;
            if(m_movesDoneThisTurn >= m_numberMaxOfMoves)
            {
                StartCoroutine(SwitchStateOnMoveComplete());
            }
        }

        IEnumerator SwitchStateOnMoveComplete()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (!m_diceIsMoveing)
                {
                    m_gamemode.SwitchToNextState();
                    break;
                }
            }
        }

        public void SetDiceIsMoving(bool p_value)
        {
            m_diceIsMoveing = p_value;
        }

        private void EmptySurroundingTiles()
        {
            if (m_surroundingTiles == null) return;
            for (int i = 0; i < m_surroundingTiles.Count; ++i)
            {
                m_surroundingTiles[i].HideOutile();
            }
            m_surroundingTiles.Clear();
        }

        private void GetSurroundingTiles()
        {
            m_surroundingTiles = m_gamemode.Grid.GetSurroundingTiles(m_currentSelectedDice.GetComponent<Dices.DiceMovementController>().GamePosition);
            for(int i = 0; i < m_surroundingTiles.Count; ++i)
            {
                if(m_surroundingTiles[i].CurrentDice == null)
                {
                    m_surroundingTiles[i].ShowOutline();
                }
            }
        }
    }
}