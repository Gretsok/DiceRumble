using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MOtter.Network
{
    public class NetworkedSceneManager : MonoBehaviour/*Fusion.Behaviour, INetworkSceneObjectProvider*/
    {
        /*private Dictionary<Guid, NetworkObject> m_sceneNetworkedObjects = new Dictionary<Guid, NetworkObject>();
        private List<SceneRef> m_currentScenes = new List<SceneRef>();
        private bool m_isLoading = false;
        public NetworkRunner Runner { get; private set; }

        #region Called by Fusion
        public void Initialize(NetworkRunner runner)
        {
            Runner = runner;
            FetchSceneRefsAndNetworkedObjects();
        }

        public bool IsReady(NetworkRunner runner)
        {
            Assert.Check(Runner == runner);
            return !m_isLoading;
        }

        public void Shutdown(NetworkRunner runner)
        {
            Assert.Check(Runner == runner);
            m_sceneNetworkedObjects.Clear();
            m_currentScenes.Clear();
            m_isLoading = false;
            Runner = null;
        }

        public bool TryResolveSceneObject(NetworkRunner runner, Guid objectGuid, out NetworkObject instance)
        {
            Assert.Check(Runner == runner);
            return m_sceneNetworkedObjects.TryGetValue(objectGuid, out instance);
        }
        #endregion

        #region Actions
        private Action m_onLevelLoaded = null;
        public void LoadLevel(LevelData.LevelData p_levelData, Action a_onLevelLoaded = null)
        {
            m_onLevelLoaded = a_onLevelLoaded;
            m_isLoading = true;
            p_levelData.OnLoadingEnded += OnLoadingEnded;
            Runner.InvokeSceneLoadStart();
            p_levelData.LoadLevel();
        }
        #endregion

        private void OnLoadingEnded(LevelData.LevelData p_levelData)
        {
            
            p_levelData.OnLoadingEnded -= OnLoadingEnded;
            FetchSceneRefsAndNetworkedObjects();
            Runner.InvokeSceneLoadDone();
            m_isLoading = false;
            m_onLevelLoaded?.Invoke();
            m_onLevelLoaded = null;
        }

        #region Utils
        private void FetchSceneRefsAndNetworkedObjects()
        {
            // Fetching SceneRefs
            m_currentScenes.Clear();
            for(int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Debug.Log("Scene "+SceneManager.GetSceneAt(i).name+" avec index : "+SceneManager.GetSceneAt(i).buildIndex);
                m_currentScenes.Add(SceneManager.GetSceneAt(i).buildIndex);
            }

            // Fetching Networked Objects
            m_sceneNetworkedObjects.Clear();
            var allNetworkedObjects = FindObjectsOfType<NetworkObject>();
            for(int i = 0; i < allNetworkedObjects.Length; ++i)
            {
                m_sceneNetworkedObjects.Add(allNetworkedObjects[i].NetworkGuid, allNetworkedObjects[i]);
            }

            // This makes the host crash when trying to load a level
            Debug.LogError("This makes the host crash when trying to load a level");
            if(m_sceneNetworkedObjects.Values.Count > 0)
                Runner.RegisterUniqueObjects(m_sceneNetworkedObjects.Values);
        }


        #endregion*/
    }
}