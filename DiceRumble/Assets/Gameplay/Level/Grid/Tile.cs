using UnityEngine;

namespace DR.Gameplay.Level.Grid
{
    public class Tile : MonoBehaviour
    {
        public Dices.Dice CurrentDice { get; private set; }

        public bool TryToSetCurrentDice(Dices.Dice a_dice)
        {
            if (CurrentDice != null) return false;
            CurrentDice = a_dice;
            return true;
        }
    }
}