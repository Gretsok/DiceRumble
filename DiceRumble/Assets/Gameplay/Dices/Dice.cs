using System;
using System.Collections.Generic;
using DR.Gameplay.Combat;
using UnityEngine;

namespace DR.Gameplay.Dices
{
    public enum EDiceType
    {
        Fire,
        Water,
        Plant,
        Poison,
        Rock,
        Neutral
    }
    
    [RequireComponent(typeof(DiceMovementController), typeof(CombatController))]
    public class Dice : MonoBehaviour
    {
        [SerializeField]
        private EDiceType m_diceType;
        public EDiceType DiceType => m_diceType;
        
        private DiceMovementController m_diceMovementController;
        public DiceMovementController DiceMovementController => m_diceMovementController;

        private CombatController m_combatController;
        public CombatController CombatController => m_combatController;

        [SerializeField]
        private List<Renderer> m_armsRenderer = null;

        private int m_teamIndex;
        public int TeamIndex => m_teamIndex;

        private void Awake()
        {
            m_diceMovementController = GetComponent<DiceMovementController>();
            m_combatController = GetComponent<CombatController>();
        }

        public void Init(int p_diceHealth, int p_teamIndex)
        {
            m_teamIndex = p_teamIndex;
            m_combatController.Init(p_diceHealth);
        }

        public void EndTurnUpdates()
        {
            m_combatController.ResetUsedPowers();
            m_diceMovementController.RemoveRootStack();
            CombatController.StatusIconsManager.RemoveStatusStack(StatusIconsManager.Status.Root, 1, false);
        }

        public void InflateArmsColors(Color a_armsColor)
        {
            m_armsRenderer.ForEach(x => x.material.color = a_armsColor);
        }
    }
}
