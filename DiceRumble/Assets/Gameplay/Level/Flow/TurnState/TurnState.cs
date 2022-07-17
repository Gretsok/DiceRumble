using System.Collections;
using MOtter.StatesMachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MOtter.SoundManagement;

namespace DR.Gameplay.Level.Flow.TurnState
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
        private bool m_diceIsMoving;

        private TurnPanel m_panel = null;

        [SerializeField]
        private SoundData m_selectionSoundData = null;

        internal override void RegisterReferences()
        {
            base.RegisterReferences();
            m_panel = GetPanel<TurnPanel>();
        }

        internal override void SetUpDependencies()
        {
            base.SetUpDependencies();
            m_panel.ShowTeamToPlay(m_gamemode.TurnManager.TurnTeam == 0);
            m_panel.ShowMovementLeft(m_numberMaxOfMoves);
            m_currentSelectedDice = null;
            DisplayOutlines();

            if (m_gamemode.TurnManager.TurnTeam == 0)
            {
                m_gamemode.DicesManager.FirstTeamDices.ForEach(x => x.GetComponent<Dices.DiceMovementController>().ResetLastTile());
            }
            else
            {
                m_gamemode.DicesManager.SecondTeamDices.ForEach(x => x.GetComponent<Dices.DiceMovementController>().ResetLastTile());
            }


        }

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
            Debug.Log("EnterState : " + gameObject.name);
            m_movesDoneThisTurn = 0;
            m_panel.ShowMovementLeft(m_numberMaxOfMoves - m_movesDoneThisTurn);
            CheckIfAtLeastOneDiceCanMove();
        }

        public override void ExitState()
        {
            base.ExitState();
            Debug.Log("ExitState : " + gameObject.name);
        }

        internal override void CleanUpDependencies()
        {
            base.CleanUpDependencies();
            ResetAllTiles();
        }

        private void HandleSelectStarted(InputAction.CallbackContext obj)
        {
            if (m_diceIsMoving) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit l_hitInfo))
            {
                if(l_hitInfo.collider.TryGetComponent(out Dices.DiceCollisionRelay l_dice)
                    && ((m_gamemode.TurnManager.TurnTeam == 0 && m_gamemode.DicesManager.FirstTeamDices.Contains(l_dice.Dice)) 
                        || (m_gamemode.TurnManager.TurnTeam == 1 && m_gamemode.DicesManager.SecondTeamDices.Contains(l_dice.Dice))))
                {
                    if (l_dice.Dice.DiceMovementController.RootStacks > 0)  //don't select a rooted dice
                        return;
                    m_currentSelectedDice = l_dice.Dice;
                    MOtter.MOtt.SOUND.Play2DSound(m_selectionSoundData);
                }
                else if(l_hitInfo.collider.TryGetComponent(out Grid.Tile l_tile))
                {
                    if(m_currentSelectedDice != null
                        && m_surroundingTiles.Contains(l_tile) 
                        && m_currentSelectedDice.GetComponent<Dices.DiceMovementController>().LastTile != l_tile
                        && m_gamemode.Grid.TryToGetTile(l_tile.GamePosition) != null
                            && l_tile.TryToSetCurrentDice(m_currentSelectedDice))
                    {         
                            MoveDiceToTile(l_tile);
                            m_currentSelectedDice = null;
                            MOtter.MOtt.SOUND.Play2DSound(m_selectionSoundData);
                    }
                    else if(l_tile.CurrentDice != null
                         && ((m_gamemode.TurnManager.TurnTeam == 0 && m_gamemode.DicesManager.FirstTeamDices.Contains(l_tile.CurrentDice))
                        || (m_gamemode.TurnManager.TurnTeam == 1 && m_gamemode.DicesManager.SecondTeamDices.Contains(l_tile.CurrentDice))))
                    {
                        m_currentSelectedDice = l_tile.CurrentDice;
                        MOtter.MOtt.SOUND.Play2DSound(m_selectionSoundData);
                    }
                }
            }
            DisplayOutlines();
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
            m_panel.ShowMovementLeft(m_numberMaxOfMoves - m_movesDoneThisTurn);
            if (m_movesDoneThisTurn >= m_numberMaxOfMoves)
            {
                StartCoroutine(SwitchStateOnMoveComplete());
            }
        }

        IEnumerator SwitchStateOnMoveComplete()
        {
            while (true)
            {
                if (!m_diceIsMoving)
                {
                    m_gamemode.SwitchToNextState();
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void SetDiceIsMoving(bool p_value)
        {
            m_diceIsMoving = p_value;
        }


        private void ResetAllTiles()
        {
            m_gamemode.Grid.ResetAllOutlines();
        }

        private void DisplayOutlines()
        {
            ResetAllTiles();
            HightlightSurroundingTiles();
            HightlightTeamDices();
        }

        private void HightlightSurroundingTiles()
        {
            if (m_currentSelectedDice == null) return;

            var moveController = m_currentSelectedDice.GetComponent<Dices.DiceMovementController>();
            if (moveController.RootStacks > 0)
                return;
            m_surroundingTiles = m_gamemode.Grid.GetSurroundingTiles(moveController.GamePosition);
            for(int i = 0; i < m_surroundingTiles.Count; ++i)
            {
                if(m_surroundingTiles[i].CurrentDice == null && m_surroundingTiles[i] != moveController.LastTile)
                {
                    m_surroundingTiles[i].ShowAsPathPossible();
                }
                else if(m_surroundingTiles[i] == moveController.LastTile)
                {
                    // Cannot go back feedback
                }                
            }
        }


        private void HightlightTeamDices()
        {
            List<Dices.Dice> dices = null;
            if(m_gamemode.TurnManager.TurnTeam == 0)
            {
                dices = m_gamemode.DicesManager.FirstTeamDices;
            }
            else
            {
                dices = m_gamemode.DicesManager.SecondTeamDices;
            }
            for(int i = 0; i < dices.Count; ++i)
            {
                if(dices[i] == m_currentSelectedDice)
                {
                    m_currentSelectedDice.GetComponent<Dices.DiceMovementController>().CurrentTile.ShowAsSelectedDice();
                }
                else if (dices[i].DiceMovementController.RootStacks > 0)
                {
                    dices[i].GetComponent<Dices.DiceMovementController>().CurrentTile.ShowAsRootedDice();
                }
                else
                {
                    dices[i].GetComponent<Dices.DiceMovementController>().CurrentTile.ShowAsTeamDice();
                }
            }
        }

        private void CheckIfAtLeastOneDiceCanMove()
        {
            List<Dices.Dice> dices = m_gamemode.TurnManager.TurnTeam == 0 ? m_gamemode.DicesManager.FirstTeamDices : m_gamemode.DicesManager.SecondTeamDices;
            foreach (Dices.Dice dice in dices)
            {
                if (dice.DiceMovementController.RootStacks == 0)
                {
                    return;
                }
            }
            Debug.Log("all dices are rooted, skipping phase");
            //no dice can move, skip phase
            m_gamemode.SwitchToNextState();
        }
    }
}