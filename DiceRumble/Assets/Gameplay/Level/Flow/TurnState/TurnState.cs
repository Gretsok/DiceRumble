using MOtter.StatesMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DR.Gameplay.Level.Flow
{
    public class TurnState : FlowState
    {
        private Dices.Dice m_currentSelectedDice = null;
        private List<Grid.Tile> m_surroundingTiles = null;
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

        private void HandleSelectStarted(InputAction.CallbackContext obj)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit l_hitInfo))
            {
                if(l_hitInfo.collider.TryGetComponent(out Dices.Dice l_dice))
                {
                    m_currentSelectedDice = l_dice;
                    GetSurroundingTiles();
                }
                else if(l_hitInfo.collider.TryGetComponent(out Grid.Tile l_tile) && m_currentSelectedDice != null)
                {
                    EmptySurroundingTiles();
                }
            }
        }

        private void EmptySurroundingTiles()
        {
            throw new NotImplementedException();
        }

        private void GetSurroundingTiles()
        {
            throw new NotImplementedException();
        }
    }
}