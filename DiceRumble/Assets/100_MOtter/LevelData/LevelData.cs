using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using MOtter.StatesMachine;

namespace MOtter.LevelData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Levels/LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private int m_dataID = 0;
        public int DataID => m_dataID;

        /// <summary>
        /// la scène qui contiendra la machine d’états principale de la scène
        /// </summary>

        [SerializeField] private SceneField m_mainScene = null;

        /// <summary>
        ///  Les additionalScenes sont également chargés, à noter que la première scène d’additionalScenes sera l’ActiveScene, elle devra donc contenir l’environnement de la scène à charger. Si il n’y a pas d’additionalScene, c’est la mainScene qui sera en Active.
        /// </summary>

        [SerializeField] 
        private SceneField[] m_additionalScenes = null;

        [SerializeField] 
        private SceneField m_loadingScreen = null;
        private bool m_loadingScreenActive = false;


        [SerializeField] 
        private LoadSceneMode m_defaultLoadMode = LoadSceneMode.Additive;
        private LoadSceneMode m_currentLoadSceneMode;
        private Scene sceneToActivate;

        AsyncOperation op;

        public Action<LevelData> OnLoadingEnded = null;

        #region Meta Data
        [SerializeField] 
        private string m_levelName = null;
        [SerializeField]
        private List<AdditionalDataInLevel> m_additionalDataInLevels = null;

        public string LevelName => m_levelName;
        #endregion

        #region LevelLoading
        public void LoadLevel()
        {
            //passe le m_default
            LoadLevel(m_defaultLoadMode);
        }

        public void LoadLevel(LoadSceneMode loadMode)
        {
            if(!MOtter.MOtterApplication.GetInstance().GAMEMANAGER.IsLoadingScenes)
            {
                op = null;

                m_currentLoadSceneMode = loadMode;
                MOtt.APP.StartCoroutine(StartLoadLevelRoutine());
            }
            else
            {
                Debug.LogError("Cannot load level when another level is loading !");
            }
        }

        IEnumerator StartLoadLevelRoutine()
        {
            yield return null;
            var currentGameMode = MOtt.GM.GetCurrentMainStateMachine<MainFlowMachine>();
            if (currentGameMode != null)
            {
                if(!m_loadingScreenActive && m_loadingScreen.SceneName != "")
                {
                    op = SceneManager.LoadSceneAsync(m_loadingScreen.SceneName, LoadSceneMode.Additive);
                    while(!op.isDone)
                    {
                        yield return null;
                    }
                    m_loadingScreenActive = true;
                }
                yield return currentGameMode.UnloadAsync(LoadScenes);
            }
            else
            {
                LoadScenes();
            }
        }

        private void LoadScenes()
        {
            //On load une scene(main + additionalScenes[])
            MOtt.GM.RegisterNewLevel(this);
            List<Scene> currentScenes = new List<Scene>();
            for(int i = 0; i< SceneManager.sceneCount; ++i)
            {
                Scene sceneToAdd = SceneManager.GetSceneAt(i);
                if(sceneToAdd.name != m_loadingScreen.SceneName)
                {
                    currentScenes.Add(sceneToAdd);
                }
                
            }
            MOtt.APP.StartCoroutine(LoadScenes(currentScenes.ToArray()));
        }

        IEnumerator LoadScenes(Scene[] loadedScenes)
        {
            yield return null;
            op = SceneManager.LoadSceneAsync(m_mainScene.SceneName, LoadSceneMode.Additive);
            Debug.Log("LOADING " + m_mainScene.SceneName);

            if(m_currentLoadSceneMode == LoadSceneMode.Single)
            {
                while (!op.isDone)
                {
                    yield return null;
                }

                for(int i = 0; i < loadedScenes.Length; ++i)
                {
                    op = SceneManager.UnloadSceneAsync(loadedScenes[i]);
                    while (!op.isDone)
                    {
                        yield return null;
                    }
                }

            }

            MOtt.APP.StartCoroutine(LoadAdditionalScenes());
        }

        IEnumerator LoadAdditionalScenes()
        {
            yield return null;
            while(!op.isDone)
            {
                yield return null;
            }
            //charger additionalScenes 
            for (int i = 0; i < m_additionalScenes.Length; i++)
            {
                op = SceneManager.LoadSceneAsync(m_additionalScenes[i].SceneName, LoadSceneMode.Additive);//empeche la supperssion de la scene precedente
                while(!op.isDone)
                {
                    yield return null;
                }
            }
            if (m_additionalScenes.Length == 0)// Si il n'y a pas d'additional scene -> m_main sera la principale 
            {
                sceneToActivate = SceneManager.GetSceneByName(m_mainScene.SceneName);
            }
            else //sinon m_additionalScenes sera la scene active
            {
                sceneToActivate = SceneManager.GetSceneByName(m_additionalScenes[0].SceneName);
            }

            OnLevelLoaded();
            while (!MOtt.GM.GetCurrentMainStateMachine<MainFlowMachine>().IsLoaded)
            {
                yield return null;
            }
            
            
            if (m_loadingScreenActive)
            {
                try
                {
                    op = SceneManager.UnloadSceneAsync(m_loadingScreen.SceneName);
                    
                }
                catch(Exception e)
                {
                    Debug.Log(e.Message);
                }
                while (!op.isDone)
                {
                    yield return null;
                }
                m_loadingScreenActive = false;

            }
            
        }

        protected virtual void OnStartedToLoadLevel()
        {

        }
        protected virtual void OnLevelLoaded()
        {
            SceneManager.SetActiveScene(sceneToActivate);
            MOtt.GM.SaveCurrentData();
            OnLoadingEnded?.Invoke(this);
        }
        #endregion

        #region Addional Data In Levels Accessors
        public T GetFirstAdditionalData<T>() where T : AdditionalDataInLevel
        {
            for(int i = 0; i < m_additionalDataInLevels.Count; ++i)
            {
                if(m_additionalDataInLevels[i] is T)
                {
                    return (T)m_additionalDataInLevels[i];
                }
            }
            Debug.LogError($"Couldn't find additional data of type {typeof(T)} in {name}");
            return (T)null;
        }

        public List<T> GetAdditionalDatas<T>() where T : AdditionalDataInLevel
        {
            List<T> datas = new List<T>();
            for (int i = 0; i < m_additionalDataInLevels.Count; ++i)
            {
                if (m_additionalDataInLevels[i] is T)
                {
                    datas.Add((T)m_additionalDataInLevels[i]);
                }
            }
            if(datas.Count == 0) 
            {
                Debug.LogError($"Couldn't find additional data of type {typeof(T)} in {name}");
            }
            return datas;
        }
        #endregion

        private void OnValidate()
        {
            List<AdditionalDataInLevel> dataToRemove = new List<AdditionalDataInLevel>();
            for(int i = 0; i < m_additionalDataInLevels.Count; ++i)
            {
                if(m_additionalDataInLevels[i] != null && m_additionalDataInLevels[i].MustBeUnique)
                {
                    for(int j = 0; j < m_additionalDataInLevels.Count; ++j)
                    {
                        if(m_additionalDataInLevels[i].GetType() == m_additionalDataInLevels[j].GetType() 
                            && m_additionalDataInLevels[i] != m_additionalDataInLevels[j])
                        {
                            Debug.LogError($"{m_additionalDataInLevels[i].GetType()} must be unique on a level data, keeping {m_additionalDataInLevels[i].name}, removing {m_additionalDataInLevels[j].name}");
                            if(!dataToRemove.Contains(m_additionalDataInLevels[j]))
                                dataToRemove.Add(m_additionalDataInLevels[j]);
                        }
                    }
                }
            }
            for(int i = dataToRemove.Count - 1; i >= 0; --i)
            {
                m_additionalDataInLevels.Remove(dataToRemove[i]);
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

#if UNITY_EDITOR
        public void SetRandomDataID()
        {
            m_dataID = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.LogWarning($"CTRL+S to save changes on {name}");
        }
#endif

    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(LevelData))]
    class LevelDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(30f);

            if (GUILayout.Button("Generate Random Data ID"))
            {
                (target as LevelData).SetRandomDataID();
            }

            GUILayout.Space(30f);

            GUILayout.Label($"Data ID : {(target as LevelData).DataID}");
        }
    }
#endif

}