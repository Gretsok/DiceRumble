using System;
using DR.Gameplay.Level.Flow;
using DR.Gameplay.Level.Grid;
using MOtter;
using MOtter.StatesMachine;
using UnityEngine;

namespace DR.Gameplay.Dices
{
    public class DiceMovementController : MonoBehaviour
    {
        public enum EDiceTopFace
        {
            ONE,
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX
        }
        #region Dice Rotations for Faces
        private Quaternion FACE_ONE = Quaternion.identity;
        private Quaternion FACE_TWO = Quaternion.Euler(0f, 0f, 90f);
        private Quaternion FACE_THREE = Quaternion.Euler(90f, 0f, 0f);
        private Quaternion FACE_FOUR = Quaternion.Euler(0f, 0f, 90f);
        private Quaternion FACE_FIVE = Quaternion.Euler(-90f, 0f, 0f);
        private Quaternion FACE_SIX = Quaternion.Euler(180f, 0f, 0f);
        #endregion

        private EDiceTopFace m_diceTopFace = default;
        public EDiceTopFace DiceTopFace => m_diceTopFace;

        [SerializeField]
        private Animations.DiceAnimationsHandler m_animationsHandler = null;
        [SerializeField]
        private Transform m_model = null;
        [SerializeField]
        private LayerMask m_facesLayerMask = default;
        private bool m_canRoll = true;

        private int m_rootStacks;
        public int RootStacks => m_rootStacks;
        public Vector2Int GamePosition { get; set; }

        public Action<DiceMovementController, Tile> OnTileChanged = null;

        private Tile m_currentTile = null;
        private Tile m_lastTile = null;
        public Tile CurrentTile => m_currentTile;
        public Tile LastTile => m_lastTile;

        private void Start()
        {
            m_animationsHandler.OnRollFinished += HandleRollFinished;
            SetTopFace();
        }

        public void ChangeTile(DiceMovementController arg1, Tile arg2)
        {
            if (arg1 != this) return;
            m_lastTile = m_currentTile;
            m_currentTile = arg2;
            OnTileChanged?.Invoke(arg1, arg2);
        }

        private void OnDestroy()
        {
            m_animationsHandler.OnRollFinished -= HandleRollFinished;
        }

        private void HandleRollFinished()
        {
            SetTopFace();
            m_canRoll = true;
            InformStateAboutMovingDice(false);
        }

        private void SetTopFace()
        {
            if (Physics.Raycast(m_model.transform.position - Vector3.up * 2f, Vector3.up, out RaycastHit l_hitInfo, 2.5f, m_facesLayerMask))
            {
                if(l_hitInfo.collider.TryGetComponent(out DiceFaceHandler l_diceFaceHandler))
                {
                    m_diceTopFace = l_diceFaceHandler.DiceTopFaceIfThisFaceIsOnGround;
                    return;
                }
            }
            Debug.LogError("Could not determine top face");
        }

        public void RollForward()
        {
            if (!m_canRoll) return;
            m_canRoll = false;
            GamePosition += Vector2Int.down;
            m_animationsHandler.TriggerRollForwardAnimations();
            InformStateAboutMovingDice(true);
        }

        public void RollBackward()
        {
            if (!m_canRoll) return;
            m_canRoll = false;
            GamePosition += Vector2Int.up;
            m_animationsHandler.TriggerRollBackwardAnimations();
            InformStateAboutMovingDice(true);
        }

        public void RollLeftward()
        {
            if (!m_canRoll) return;
            m_canRoll = false;
            GamePosition += Vector2Int.left;
            m_animationsHandler.TriggerRollLeftwardAnimations();
            InformStateAboutMovingDice(true);
        }

        public void RollRightward()
        {
            if (!m_canRoll) return;
            m_canRoll = false;
            GamePosition += Vector2Int.right;
            m_animationsHandler.TriggerRollRightwardAnimations();
            InformStateAboutMovingDice(true);
        }

        private void InformStateAboutMovingDice(bool p_moving)
        {
            StateMonoBehaviour state = MOtt.GM.GetCurrentMainStateMachine<LevelGameMode>().GetCurrentState();
            if (state && state.GetType() == typeof(TurnState))
            {
                ((TurnState)state).SetDiceIsMoving(p_moving);
            }        
        }

        public void RemoveRootStack()
        {
            if(m_rootStacks > 0)
                m_rootStacks--;
        }

        public void ApplyRoot(int p_stacks)
        {
            m_rootStacks = p_stacks;
        }
    }
}