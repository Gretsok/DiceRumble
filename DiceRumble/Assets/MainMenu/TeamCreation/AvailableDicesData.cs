using System.Collections.Generic;
using UnityEngine;

namespace DR.MainMenu.TeamCreation
{
    [CreateAssetMenu(fileName = "AvailableDicesData", menuName = "Dice Rumble/MainMenu/TeamCreation/AvailableDicesData")]
    public class AvailableDicesData : ScriptableObject
    {
        [SerializeField]
        private List<Gameplay.Dices.DiceData> m_availableDicesList = null;
        public List<Gameplay.Dices.DiceData> AvailableDicesList => m_availableDicesList;
    }
}