using UnityEngine;

namespace DR.Gameplay.Data
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "Dice Rumble/Gameplay/GameplayData")]
    public class GameplayData : ScriptableObject
    {
        public int WaterHealAmount;
        public int PoisonDamage;
        public int FireDamagePerStack;
        public int RockShieldPerStack;
        public int PlantRootPerStack;
    }
}
