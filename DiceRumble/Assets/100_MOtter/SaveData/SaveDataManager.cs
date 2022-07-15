using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MOtter.Save
{
    [System.Serializable]
    public class SaveDataManager
    {
        [System.Serializable]
        public struct SaveDataElement
        {
            public string SaveName;
            public string FileName;
        }
        public string FileName = "SaveData.dat";

        public List<SaveDataElement> SaveDataList = new List<SaveDataElement>();
        public float CameraSensitivity = 15f;

        public float SFXSoundSensitivity = 0.5f;
        public float MusicSoundSensitivity = 0.5f;

        public SaveDataManager()
        {

        }

        public void Load()
        {
            LoadFromFile(FileName, out string jsonContent);
            LoadFromJson(jsonContent);
            if(MOtt.SOUND != null)
            {
                MOtt.SOUND.SetVolume(SFXSoundSensitivity, SoundManagement.ESoundCategoryName.SFX);
                MOtt.SOUND.SetVolume(MusicSoundSensitivity, SoundManagement.ESoundCategoryName.Music);
            }
            
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string a_Json)
        {
            JsonUtility.FromJsonOverwrite(a_Json, this);
        }

        public void SaveSaveDataManager()
        {
            SFXSoundSensitivity = MOtt.SOUND.GetVolume(SoundManagement.ESoundCategoryName.SFX);
            MusicSoundSensitivity = MOtt.SOUND.GetVolume(SoundManagement.ESoundCategoryName.Music);

            WriteToFile(FileName, ToJson());
        }


        #region SaveData Management

        public void SaveSaveData(SaveData saveData)
        {
            // If we find existing save for this saveData

            if (SaveDataList.FindAll(x => x.SaveName == saveData.SaveName).Count > 0)
            {
                SaveDataElement dataElement = SaveDataList.Find(x => x.SaveName == saveData.SaveName);
                if (WriteToFile(dataElement.FileName, saveData.ToJson()))
                {
                    Debug.Log("Save successful");
                }
                else
                {
                    Debug.LogError("Error while trying to save on existing save file");
                }
            }
            else
            {
                SaveDataElement newSaveDataElement;
                newSaveDataElement.SaveName = saveData.SaveName;
                newSaveDataElement.FileName = saveData.SaveName + "PlayerSaveData.dat";
                SaveDataList.Add(newSaveDataElement);
                if (WriteToFile(newSaveDataElement.FileName, saveData.ToJson()))
                {
                    Debug.Log("New save successful");
                }
                else
                {
                    Debug.LogError("Error while trying to save on new save file");
                }
            }
        }

        public SaveData LoadSaveData(string saveDataName)
        {
            if (SaveDataList.FindAll(x => x.SaveName == saveDataName).Count > 0)
            {
                SaveDataElement dataElement = SaveDataList.Find(x => x.SaveName == saveDataName);
                if (LoadFromFile(dataElement.FileName, out string jsonContent))
                {
                    SaveData saveData = new SaveData();
                    saveData.LoadFromJson(jsonContent);
                    return saveData;
                }
                else
                {
                    Debug.LogError($"Error while trying to load save named : {saveDataName} and file named : {dataElement.FileName}");
                    return null;
                }
            }
            else
            {
                Debug.LogError($"Cannot load save -> No save data named : {saveDataName}");
                return null;
            }
        }

        public void RemoveSaveData(SaveData saveData)
        {
            if (SaveDataList.FindAll(x => x.SaveName == saveData.SaveName).Count > 0)
            {
                try
                {
                    SaveDataElement dataElement = SaveDataList.Find(x => x.SaveName == saveData.SaveName);
                    File.Delete(Path.Combine(Application.persistentDataPath, dataElement.FileName));
                    Debug.Log("Delete save data");
                    SaveDataList.Remove(dataElement);
                    SaveSaveDataManager();
                }
                catch (Exception)
                {
                    Debug.LogError("Error while deleting save data");
                }
            }
        }
        #endregion

        #region Reading And Writing Files
        public static bool WriteToFile(string a_FileName, string a_FileContents)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, a_FileName);
            try
            {
                CreateFileIfDoesntExist(fullPath);
                File.WriteAllText(fullPath, a_FileContents);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {fullPath} with exception {e}");
            }
            return false;
        }

        public static bool LoadFromFile(string a_FileName, out string result)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, a_FileName);
            try
            {
                CreateFileIfDoesntExist(fullPath);
                result = File.ReadAllText(fullPath);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read from {fullPath} with exception {e.InnerException}");
                result = "";
                return false;
            }
        }

        private static void CreateFileIfDoesntExist(string path)
        {
            if (!File.Exists(path))
            {
                FileStream stream = File.Create(path);
                stream.Close();
            }
        }
        #endregion
    }
}