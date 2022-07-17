using System;
using System.Collections.Generic;
using DR.Gameplay.Combat.UI;
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
        [SerializeField] private StatusIconsManager m_statusIconsManager;
        [SerializeField] private LifeBarWidget m_lifeBarWidget;
        public StatusIconsManager StatusIconsManager => m_statusIconsManager;
        
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
        public Action<CombatController, int, EDiceType> OnDamageTaken = null;

        private void Start()
        {
            m_dice = GetComponent<Dice>();
        }

        public void Attack(CombatController p_target)
        {
            int myDamages = GetDamages();
            myDamages *= 2; //Attacker has double damage
            EDiceType dmgType = EDiceType.Neutral;
            if (m_fireStacks > 0)
            {
                m_usedFirePowerThisTurn = true;
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

        public void TakeDamage(int p_damage, EDiceType p_damageType)
        {
            int shieldLost = 0;
            while (m_shield > 0 && p_damage > 0)
            {
                m_shield--;
                p_damage--;
                shieldLost++;
            }
            m_currentHealth -= p_damage;
            if (m_currentHealth <= 0)
                m_currentHealth = 0;
            OnLifeUpdated?.Invoke(this);
            OnDamageTaken?.Invoke(this, p_damage, p_damageType);
            if (shieldLost > 0)
            {
                StatusIconsManager.RemoveStatusStack(StatusIconsManager.Status.Shield, shieldLost, false);
            }
        }
        
        public int GetDamages()
        {
            return (int)m_dice.DiceMovementController.DiceTopFace + 1;
        }

        public void Init(int p_diceHealth, int p_teamIndex)
        {
            SetBaseHealth(p_diceHealth);
            SetCurrentHealth(p_diceHealth);
            m_lifeBarWidget.SetupTeamColor(p_teamIndex);
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
                StatusIconsManager.RemoveStatusStack(StatusIconsManager.Status.Fire, 0, true);
            }
            if (m_usedPlantPowerThisTurn)
            {
                m_plantStacks = 0;
                m_usedPlantPowerThisTurn = false;
                StatusIconsManager.RemoveStatusStack(StatusIconsManager.Status.Plant, 0, true);
            }
        }

        public void Poison()
        {
            m_poisoned = true;
        }
        
        public void Die()
        {
            Debug.Log("ME DEAD!");
            var moveController = GetComponent<DiceMovementController>();
            moveController.ChangeTile(moveController, null);
            MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().DicesManager.HandleDiceDeath(GetComponent<Dice>());
            Destroy(gameObject);
        }

        private void ApplyCombatEffects(CombatController p_target)
        {
            if (m_plantStacks > 0)
            {
                p_target.Dice.DiceMovementController.ApplyRoot(m_plantStacks);
                p_target.StatusIconsManager.AddStatusStack(StatusIconsManager.Status.Root, m_plantStacks);
                m_usedPlantPowerThisTurn = true;
            }
            if (m_dice.DiceType is EDiceType.Poison)
            {
                p_target.Poison();
                p_target.StatusIconsManager.AddStatusStack(StatusIconsManager.Status.Poison, 1);
            }
        }
        
        public void UsePower()
        {
            Debug.Log("Use power on " + gameObject.name);
            DicesManager dm = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().DicesManager;
            
            switch (m_dice.DiceType)
            {
                case EDiceType.Fire:
                    int fireStacksToAdd = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GameplayData.FireDamagePerStack;
                    m_fireStacks += fireStacksToAdd;
                    m_statusIconsManager.AddStatusStack(StatusIconsManager.Status.Fire, fireStacksToAdd);
                    break;
                case EDiceType.Plant:
                    int plantStacksToAdd = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GameplayData.PlantRootPerStack;
                    m_plantStacks += plantStacksToAdd;
                    m_statusIconsManager.AddStatusStack(StatusIconsManager.Status.Plant, plantStacksToAdd);
                    break;
                case EDiceType.Poison:
                    int poisonDamage = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GameplayData.PoisonDamage;
                    List<Dice> enemyDices = m_dice.TeamIndex == 0 ? dm.SecondTeamDices : dm.FirstTeamDices;
                    foreach (Dice dice in enemyDices)
                    {
                        if (dice.CombatController.Poisoned)
                        {
                            dice.CombatController.TakeDamage(poisonDamage, EDiceType.Poison);
                        }
                    }
                    break;
                case EDiceType.Water:
                    int healAmount = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GameplayData.WaterHealAmount;
                    List<Dice> teamDices = m_dice.TeamIndex == 0 ? dm.FirstTeamDices : dm.SecondTeamDices;
                    foreach (Dice dice in teamDices)
                    {
                        if (dice != m_dice)
                        {
                            dice.CombatController.Heal(healAmount);
                            dice.CombatController.OnDamageTaken?.Invoke(dice.CombatController, healAmount, EDiceType.Water);
                        }
                    }
                    break;
                case EDiceType.Rock:
                    int shieldStacksToAdd = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GameplayData.RockShieldPerStack;
                    m_shield += shieldStacksToAdd;
                    m_statusIconsManager.AddStatusStack(StatusIconsManager.Status.Shield, shieldStacksToAdd);
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
