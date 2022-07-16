using System;
using DR.Gameplay.Dices;
using UnityEngine;

namespace DR.Gameplay.Combat
{
    [RequireComponent(typeof(Dice))]
    public class CombatController : MonoBehaviour
    {
        //Health
        private int m_baseHealth;
        public int BaseHealth => m_baseHealth;
        private int m_currentHealth;
        public int CurrentHealth => m_currentHealth;

        private Dice m_dice;

        private void Start()
        {
            m_dice = GetComponent<Dice>();
        }

        public void Attack(CombatController p_target)
        {
            int dmgToDeal = GetDamages() - p_target.GetDamages();
            if (dmgToDeal > 0)
            {
                p_target.TakeDamage(dmgToDeal);
            }
            else if(dmgToDeal < 0)
            {
                TakeDamage(-dmgToDeal);
            }
        }

        public void TakeDamage(int p_damage)
        {
            m_currentHealth -= p_damage;
            if (m_currentHealth <= 0)
            {
                Die();
            }
        }
        
        public int GetDamages()
        {
            return (int)m_dice.DiceMovementController.DiceTopFace + 1;
        }

        public void Init(int p_diceHealth)
        {
            SetBaseHealth(p_diceHealth);
            SetCurrentHealth(p_diceHealth);
        }
        
        private void SetBaseHealth(int p_baseHealth)
        {
            m_baseHealth = p_baseHealth;
        }
        
        private void SetCurrentHealth(int p_currentHealth)
        {
            m_currentHealth = p_currentHealth;
        }
        
        private void Die()
        {
            Debug.Log("ME DEAD!");
        }
    }
}
