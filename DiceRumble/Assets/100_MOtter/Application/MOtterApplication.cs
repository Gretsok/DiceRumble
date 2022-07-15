using MOtter.Context;
using MOtter.DataSceneConveyance;
using MOtter.Localization;
using MOtter.PlayersManagement;
using MOtter.Save;
using MOtter.SoundManagement;
using MOtter.StatesMachine;
using MOtter.Utils;
using UnityEngine;

namespace MOtter
{
    public class MOtterApplication : MonoBehaviour
    {
        private void Awake()
        {
            s_instance = this;
            DontDestroyOnLoad(s_instance);
            s_instance.m_gameManager.Init();
        }

        private static MOtterApplication s_instance;
        public static MOtterApplication GetInstance()
        {
            if (s_instance == null)
            {
                var app_GO = Instantiate(Resources.Load<GameObject>("MOtterApplication"));
                s_instance = app_GO.GetComponent<MOtterApplication>();
                DontDestroyOnLoad(s_instance);
                Debug.Log("MOtterApplication instance created");
            }
            return s_instance;
        }

        [SerializeField] private GameManager m_gameManager = null;
        [SerializeField] private LocalizationManager m_localizationManager = null;
        [SerializeField] private SoundManager m_soundManager = null;
        [SerializeField] private PlayerProfileManager m_playerProfileManager = null;
        [SerializeField] private ContextManager m_contextManager = null;
        private DataSceneConveyor m_dataSceneConveyor = new DataSceneConveyor();
        
        public GameManager GAMEMANAGER => m_gameManager;
        public LocalizationManager LOCALIZATION => m_localizationManager;
        public SoundManager SOUND => m_soundManager;
        public PlayerProfileManager PLAYERPROFILES => m_playerProfileManager;
        public ContextManager CONTEXT => m_contextManager;
        public MOtterUtils UTILS { get; } = new MOtterUtils();
        public SaveDataManager SAVE => GAMEMANAGER.SaveDataManager;
        public DataSceneConveyor SCENECONVEYOR => m_dataSceneConveyor;

    }
}
