using System;
using System.Collections.Generic;
using DR.Gameplay.Dices;
using DR.Gameplay.Dices.Manager;
using DR.Gameplay.Level.Flow;
using MOtter;
using UnityEngine;

namespace DR.Gameplay.Combat
{
    [RequireComponent(typeof(Dice))]
    public class CombatController : MonoBehaviour
    {
        private int m_baseHealth;
        public int BaseHealth => m_baseHealth;
        private int m_currentHealth;
        public int CurrentHealth => m_currentHealth;
        private int m_shield;
        public int Shield => m_shield;
        private int m_fireStacks;
        public int FireStacks => m_fireStacks;
        private bool m_usedFirePowerThisTurn;
        public bool UsedFirePowerThisTurn => m_usedFirePowerThisTurn;
        private int m_plantStacks;
        public int PlantStacks => m_plantStacks;
        private bool m_usedPlantPowerThisTurn;
        public bool UsedPlantPowerThisTurn => m_usedPlantPowerThisTurn;
        private bool m_poisoned;
        public bool Poisoned => m_poisoned;

        private Dice m_dice;
        public Dice Dice => m_dice;

        public Action<CombatController> OnLifeUpdated = null;
        public Action<CombatController, int> OnDamageTaken = null;

        private void Start()
        {
            m_dice = GetComponent<Dice>();
        }

        public void Attack(CombatController p_target)
        {
            int myDamages = GetDamages();
            if (myDamages == 6)
            {
                UsePower();
            }
            EDiceType dmgType = EDiceType.Neutral;
            if (m_fireStacks > 0)
            {
                myDamages += m_fireStacks;
                dmgType = EDiceType.Fire;
            }
            ApplyCombatEffects(p_target);
            int dmgToDeal = myDamages - p_target.GetDamages();
            Debug.Log("Damage To Inflict : " + dmgToDeal);
            if (dmgToDeal > 0)
            {
                p_target.TakeDamage(dmgToDeal, dmgType);
            }
            else if(dmgToDeal < 0)
            {
                TakeDamage(-dmgToDeal, EDiceType.Neutral);
            }
        }

        public void TakeDamage(int p_damage, EDiceType p_diceType)
        {
            while (m_shield > 0 && p_damage > 0)
            {
                m_shield--;
                p_damage--;
            }
            m_currentHealth -= p_damage;
            if (m_currentHealth <= 0)
                m_currentHealth = 0;
            OnLifeUpdated?.Invoke(this);
            OnDamageTaken?.Invoke(this, p_damage);
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

        public void Heal(int p_amount)
        {
            int newHealth = m_currentHealth + p_amount;
            if (newHealth > m_baseHealth)
            {
                newHealth = m_baseHealth;
            }
            SetCurrentHealth(newHealth);
        }

        public void ResetUsedPowers()
        {
            if (m_usedFirePowerThisTurn)
            {
                m_fireStacks = 0;
                m_usedFirePowerThisTurn = false;
            }
            if (m_usedPlantPowerThisTurn)
            {
                m_plantStacks = 0;
                m_usedPlantPowerThisTurn = false;
            }
        }

        public void Poison()
        {
            m_poisoned = true;
        }
        
        public void Die()
        {
            Debug.Log("ME DEAD!");
        }

        private void ApplyCombatEffects(CombatController p_target)
        {
            if (m_plantStacks > 0)
            {
                p_target.Dice.DiceMovementController.ApplyRoot(m_plantStacks);
                m_usedPlantPowerThisTurn = true;
            }
            if (m_dice.DiceType is EDiceType.Poison)
            {
                p_target.Poison();
            }
        }
        
        private void UsePower()
        {
            DicesManager dm = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().DicesManager;
            List<Dice> teamDices;
            
            switch (m_dice.DiceType)
            {
                case EDiceType.Fire:
                    m_fireStacks += MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GameplayData.FireDamagePerStack;
                    break;
                case EDiceType.Plant:
                    m_plantStacks += MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GameplayData.PlantRootPerStack;
                    break;
                case EDiceType.Poison:
                    int poisonDamage = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GameplayData.PoisonDamage;
                    teamDices = m_dice.TeamIndex == 0 ? dm.FirstTeamDices : dm.SecondTeamDices;
                    foreach (Dice dice in teamDices)
                    {
                        if (dice.CombatController.Poisoned)
                        {
                            dice.CombatController.TakeDamage(poisonDamage, EDiceType.Poison);
                        }
                    }
                    break;
                case EDiceType.Water:
                    int healAmount = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GameplayData.WaterHealAmount;
                    teamDices = m_dice.TeamIndex == 0 ? dm.FirstTeamDices : dm.SecondTeamDices;
                    foreach (Dice dice in teamDices)
                    {
                        if(dice != m_dice)
                            dice.CombatController.Heal(healAmount);
                    }
                    break;
                case EDiceType.Rock:
                    m_shield += MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GameplayData.RockShieldPerStack;
                    break;
            }
        }
        
        private void SetBaseHealth(int p_baseHealth)
        {
            m_baseHealth = p_baseHealth;
        }
        
        private void SetCurrentHealth(int p_currentHealth)
        {
            m_currentHealth = p_currentHealth;
            OnLifeUpdated?.Invoke(this);
        }
    }
}
