using DR.Gameplay.Dices;
using UnityEngine;

namespace DR.Gameplay.Level.Grid
{
    public class Tile : MonoBehaviour
    {
        public Dices.Dice CurrentDice { get; private set; }
        [SerializeField]
        private Vector2Int m_gamePosition = default;
        [SerializeField]
        private GameObject m_outline = null;
        public Vector2Int GamePosition 
        { 
            get
            {
                return m_gamePosition;
            }
            set
            {
                m_gamePosition = value;
            } 
        }

        public bool TryToSetCurrentDice(Dice a_dice)
        {
            if (CurrentDice != null) return false;
            CurrentDice = a_dice;
            var moveController = CurrentDice.GetComponent<DiceMovementController>();
            moveController.OnTileChanged?.Invoke(moveController);
            moveController.OnTileChanged += HandleDiceMoved;
            return true;
        }

        private void HandleDiceMoved(DiceMovementController obj)
        {
            obj.OnTileChanged -= HandleDiceMoved;
            CurrentDice = null;
        }

        public void ShowOutline()
        {
            m_outline.SetActive(true);
        }

        public void HideOutile()
        {
            m_outline.SetActive(false);
        }
    }
}