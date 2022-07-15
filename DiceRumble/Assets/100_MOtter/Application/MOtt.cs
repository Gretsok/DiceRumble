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
    public class MOtt : MonoBehaviour
    {
        public static MOtterApplication APP => MOtterApplication.GetInstance();
        public static GameManager GM => MOtterApplication.GetInstance().GAMEMANAGER;
        public static LocalizationManager LANG => MOtterApplication.GetInstance().LOCALIZATION;
        public static SoundManager SOUND => MOtterApplication.GetInstance().SOUND;
        public static PlayerProfileManager PLAYERS => MOtterApplication.GetInstance().PLAYERPROFILES;
        public static ContextManager CONTEXT => MOtterApplication.GetInstance().CONTEXT;
        public static MOtterUtils UTILS => MOtterApplication.GetInstance().UTILS;
        public static SaveDataManager SAVE => MOtterApplication.GetInstance().GAMEMANAGER.SaveDataManager;
        public static DataSceneConveyor DATACONVEY => MOtterApplication.GetInstance().SCENECONVEYOR;
    }
}