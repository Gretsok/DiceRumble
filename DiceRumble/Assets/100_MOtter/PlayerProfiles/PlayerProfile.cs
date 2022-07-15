using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MOtter.PlayersManagement
{
    [System.Serializable]
    public class PlayerProfile : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput m_playerInput = null;


        private int m_index = 0;

        public int Index => m_index;

        public Action<EDeviceType> OnDeviceTypeChanged = null;

        public InputActionAsset Actions => m_playerInput.actions;

        public void Init(int index)
        {
            m_index = index;
            m_playerInput.onControlsChanged += M_playerInput_onControlsChanged;
        }

        private void M_playerInput_onControlsChanged(PlayerInput a_playerInput)
        {
            Debug.Log($"Control Scheme changed : {a_playerInput.currentControlScheme}");
            OnDeviceTypeChanged?.Invoke(GetCurrentDeviceType());
        }


        public EDeviceType GetCurrentDeviceType()
        {
            switch(m_playerInput.currentControlScheme)
            {
                case "MouseAndKeyboard":
                    return EDeviceType.MouseAndKeyboard;
                case "Gamepad":
                    return EDeviceType.Gamepad;
                default:
                    return EDeviceType.None;
            }
        }

        public void Clear()
        {
            m_playerInput.onControlsChanged -= M_playerInput_onControlsChanged;
        }

    }

    public enum EDeviceType
    {
        None,
        MouseAndKeyboard,
        Gamepad
    }
}