using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MOtter.PlayersManagement
{
    public class PlayerProfileManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputManager m_inputManager = null;

        private List<PlayerProfile> m_instantiatedPlayerProfiles = new List<PlayerProfile>();

        public List<PlayerProfile> PlayerProfiles => m_instantiatedPlayerProfiles;

        public Action<PlayerProfile> OnPlayerJoined = null;
        public Action<PlayerProfile> OnPlayerLeft = null;

        private void Awake()
        {
            m_inputManager.onPlayerJoined += OnPlayerJoinedInputManager;
            m_inputManager.onPlayerLeft += OnPlayerLeftInputManager;
        }

        public void SetJoinBehaviour(PlayerJoinBehavior joinBehavior)
        {
            switch(m_inputManager.joinBehavior)
            {
                case PlayerJoinBehavior.JoinPlayersManually:
                    break;
                case PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed:
                    
                    break;
                case PlayerJoinBehavior.JoinPlayersWhenJoinActionIsTriggered:
                    break;
            }

            m_inputManager.joinBehavior = joinBehavior;

            switch (m_inputManager.joinBehavior)
            {
                case PlayerJoinBehavior.JoinPlayersManually:
                    break;
                case PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed:
                    
                    break;
                case PlayerJoinBehavior.JoinPlayersWhenJoinActionIsTriggered:
                    break;
            }
        }

        private void OnPlayerLeftInputManager(PlayerInput obj)
        {
            if(obj.TryGetComponent<PlayerProfile>(out PlayerProfile profile))
            {
                OnPlayerLeft?.Invoke(profile);
                m_instantiatedPlayerProfiles.Remove(profile);
            }
        }

        private void OnPlayerJoinedInputManager(PlayerInput obj)
        {
            if (obj.TryGetComponent<PlayerProfile>(out PlayerProfile profile))
            {
                obj.transform.SetParent(transform);
                obj.actions.Enable();
                ManuallyAddNewPlayer(profile);
                OnPlayerJoined?.Invoke(profile);
            }
        }

        public void AddNewPlayerOnCallbackContext(InputAction.CallbackContext context)
        {
            m_inputManager.JoinPlayerFromActionIfNotAlreadyJoined(context);
        }

        private PlayerProfile ManuallyAddNewPlayer(PlayerProfile newPlayer)
        {
            bool indexFree = false;
            int indexToSet = -1;

            do
            {
                indexToSet++;
                indexFree = true;
                for(int i = 0; i < PlayerProfiles.Count; ++i)
                {
                    if(PlayerProfiles[i].Index == indexToSet)
                    {
                        indexFree = false;
                        break;
                    }
                }
            } while (!indexFree);

            newPlayer.Init(indexToSet);
            m_instantiatedPlayerProfiles.Add(newPlayer);
            Debug.Log($"Created new player with Index {newPlayer.Index}");

            return newPlayer;
        }

        public InputActionAsset GetActions(int playerIndex)
        {
            return PlayerProfiles[playerIndex].Actions;
        }

    
        public void RemovePlayer(int playerIndex)
        {
            RemovePlayer(GetPlayerByIndex(playerIndex));
        }

        public void RemovePlayer(PlayerProfile playerProfile)
        {
            playerProfile.Clear();
            m_instantiatedPlayerProfiles.Remove(playerProfile);
            if(playerProfile != null)
            {
                Destroy(playerProfile.gameObject);
            }
            
        }

        public PlayerProfile GetPlayerByIndex(int index)
        {
            return PlayerProfiles.Find(x => x.Index == index);
        }

        private void OnDestroy()
        {
            m_inputManager.onPlayerJoined -= OnPlayerJoinedInputManager;
            m_inputManager.onPlayerLeft -= OnPlayerLeftInputManager;
        }

    }
}
