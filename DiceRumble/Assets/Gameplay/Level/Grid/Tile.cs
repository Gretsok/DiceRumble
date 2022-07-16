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
        private TileSelectorController m_outline = null;
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
            moveController.ChangeTile(moveController, this);
            moveController.OnTileChanged += HandleDiceMoved;
            return true;
        }

        private void HandleDiceMoved(DiceMovementController obj, Tile arg1)
        {
            obj.OnTileChanged -= HandleDiceMoved;
            CurrentDice = null;
        }

        public void ShowAsPathPossible()
        {
            m_outline.SetUpAsPathPossible();
            m_outline.gameObject.SetActive(true);
        }

        public void ShowAsTeamDice()
        {
            m_outline.SetUpAsTeamDice();
            m_outline.gameObject.SetActive(true);
        }

        public void ShowAsSelectedDice()
        {
            m_outline.SetUpAsSelectedDice();
            m_outline.gameObject.SetActive(true);
        }

        public void ShowAsNothingSpecial()
        {
            m_outline.gameObject.SetActive(false);
        }
    }
}