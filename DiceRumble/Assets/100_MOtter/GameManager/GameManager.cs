using System;
using System.Collections;
using MOtter.Save;
using System.Collections.Generic;
using UnityEngine;

namespace MOtter.StatesMachine
{
    public class GameManager : MonoBehaviour
    {
        private MainFlowMachine m_flowMachine = null;
        private List<LevelData.LevelData> m_currentSceneData = new List<LevelData.LevelData>();

        private SaveDataManager m_saveDataManager = null;
        public SaveDataManager SaveDataManager => m_saveDataManager;

        private SaveData m_currentSaveData = null;

        private bool m_isLoadingScenes = false;
        public bool IsLoadingScenes => m_isLoadingScenes;

        internal void Init()
        {
            m_saveDataManager = new SaveDataManager();
            m_saveDataManager.Load();
        }

        private void Update()
        {
            if (m_flowMachine != null)
            {
                if(m_flowMachine.IsLoaded)
                {
                    m_flowMachine.DoUpdate();
                }  
            }
        }

        private void FixedUpdate()
        {
            if (m_flowMachine != null)
            {
                if (m_flowMachine.IsLoaded)
                {
                    m_flowMachine.DoFixedUpdate();
                }
            }
        }

        private void LateUpdate()
        {
            if (m_flowMachine != null)
            {
                if (m_flowMachine.IsLoaded)
                {
                    m_flowMachine.DoLateUpdate();
                }
            }
        }

        internal void RegisterFlowMachine(MainFlowMachine statesmachine)
        {
            m_flowMachine = statesmachine;
            StartCoroutine(WaitToStartGMLoadAsync(statesmachine));
        }

        private IEnumerator WaitToStartGMLoadAsync(MainFlowMachine statesmachine)
        {
            while(m_isLoadingScenes)
            {
                yield return null;
            }
            statesmachine.LevelData = m_currentSceneData[m_currentSceneData.Count - 1];
            StartCoroutine(statesmachine.LoadAsync());
        }

        public T GetCurrentMainStateMachine<T>() where T : MainFlowMachine
        {
            if (m_flowMachine != null && m_flowMachine is T)
            {
                return (T) m_flowMachine;
            }
            return null;
        }

        #region SceneDataManagement
        public void RegisterNewLevel(LevelData.LevelData sceneData, bool isAdditive = false)
        {
            if(!isAdditive)
            {
                m_currentSceneData.Clear();
            }
            m_currentSceneData.Add(sceneData);
            m_isLoadingScenes = true;
            sceneData.OnLoadingEnded += OnLoadingEnded;
        }

        private void OnLoadingEnded(LevelData.LevelData obj)
        {
            m_isLoadingScenes = false;
        }

        #endregion

        #region SaveManagement
        public void UseSaveData(SaveData saveData)
        {
            m_currentSaveData = saveData;
        }

        public void SaveCurrentData()
        {
            if(m_currentSaveData != null)
            {
                m_saveDataManager.SaveSaveData(m_currentSaveData);
            }
        }

        public T GetSaveData<T>() where T : SaveData
        {
            return (m_currentSaveData as T);
        }
        #endregion
    }
}