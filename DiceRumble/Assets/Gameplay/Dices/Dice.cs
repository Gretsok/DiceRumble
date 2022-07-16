using System;
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
    }
    
    [RequireComponent(typeof(DiceMovementController), typeof(CombatController))]
    public class Dice : MonoBehaviour
    {
        private EDiceType m_eDiceType;
        public EDiceType EDiceType => m_eDiceType;
        
        private DiceMovementController m_diceMovementController;
        public DiceMovementController DiceMovementController => m_diceMovementController;

        private CombatController m_combatController;
        public CombatController CombatController => m_combatController;

        private void Awake()
        {
            m_diceMovementController = GetComponent<DiceMovementController>();
            m_combatController = GetComponent<CombatController>();
        }

        public void Init(int p_diceHealth)
        {
            m_combatController.Init(p_diceHealth);
        }
    }
}
