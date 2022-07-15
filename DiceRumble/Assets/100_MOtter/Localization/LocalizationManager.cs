using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;



namespace MOtter.Localization
{

    public class LocalizationManager : MonoBehaviour
    {
        private const string path = "Assets/Resources/Localization.tsv";
        #region Fields
        private bool isInit = false;

        #region Language Management
        private int m_currentLanguageIndex = 0;
        private Dictionary<string, int> m_indexesOfLanguagesShortNames = new Dictionary<string, int>();
        [SerializeField]
        private AllLanguagesData m_allLanguagesData = null;
        private LanguageData m_currentLanguageData = null;
        #endregion

        #region Localizers
        private List<TextLocalizer> m_registeredTextLocalizers = new List<TextLocalizer>();
        #endregion
        #endregion

        #region Methods
        #region Language Management
        private void Start()
        { 
            if (!isInit)
            {
                Init();
            }
        }

        private void Init()
        {
            SwitchLanguage(m_currentLanguageIndex);
            isInit = true;
        }

        public void SwitchLanguage(int languageIndex)
        {
            m_currentLanguageIndex = GetValidIndex(languageIndex);
            Debug.Log("language index : " + m_currentLanguageIndex);
            m_currentLanguageData = LoadCurrentLanguage();
            UpdateLocalizers();
        }

        public void SwitchToNextLanguage()
        {
            SwitchLanguage(m_currentLanguageIndex + 1);
        }

        public void SwitchToPreviousLanguage()
        {
            SwitchLanguage(m_currentLanguageIndex - 1);
        }

        private LanguageData LoadLanguage(int index)
        {
            return m_allLanguagesData.GetLanguageData(index);
        }
        

        /// <summary>
        /// Returns the dictonary of the current Language
        /// </summary>
        /// <param name="languageIndex"></param>
        /// <returns></returns>
        private LanguageData LoadCurrentLanguage()
        {
            return LoadLanguage(m_currentLanguageIndex);
        }
        #endregion

        #region ReadingLanguageManagement
        /// <summary>
        /// Returns the dictonary of the language defined by languageIndex
        /// </summary>
        /// <param name="languageIndex"></param>
        /// <returns></returns>
        private LanguageDictionary ReadLanguage(int languageIndex)
        {
            LanguageDictionary languageDictionary = new LanguageDictionary();
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string key = "";
                string translation = "";


                int charIndex = 0;
                // Getting the key
                while (charIndex < line.Length && line[charIndex] != '\t')
                {
                    key = key + line[charIndex];
                    charIndex++;
                }

                int browsedLanguageIndex = 0;
                // Browsing to the language we want to load
                while (browsedLanguageIndex < languageIndex)
                {
                    charIndex++;
                    if (charIndex >= line.Length)
                    {
                        Debug.LogError("Language Index doesn't exist");
                    }
                    else if (line[charIndex] == '\t')
                    {
                        browsedLanguageIndex++;
                    }
                }
                charIndex++;

                // Getting the translation of the key in the language selected
                while (charIndex < line.Length && line[charIndex] != '\t')
                {
                    translation = translation + line[charIndex];
                    charIndex++;
                }
                try
                {
                    if (key != string.Empty)
                    {
                        languageDictionary.Add(key, translation);
                    }
                }
                catch(System.Exception e)
                {
                    Debug.LogError($"{e.Message} \n {e.StackTrace} \n key: {key}");
                }
                
                Debug.Log("Add " + translation + " for " + key);
            }
            reader.Close();
            return languageDictionary;
        }
        #endregion

        #region Localizers
        /// <summary>
        /// Add a TextLocalizer to the Localization system to update it
        /// </summary>
        /// <param name="textLocalizer"></param>
        public void RegisterTextLocalizer(TextLocalizer textLocalizer)
        {
            if (!isInit)
            {
                Init();
            }
            Debug.Log("Register new text localizer on " + textLocalizer.name);
            m_registeredTextLocalizers.Add(textLocalizer);
            UpdateLocalizers();
        }

        /// <summary>
        /// Remove a TextLocalizer to the Localization system
        /// </summary>
        /// <param name="textLocalizer"></param>
        public void UnregisterTextLocalizer(TextLocalizer textLocalizer)
        {
            int textLocalizerToRemoveIndex = -1;
            Debug.Log("Unregister new text localizer on " + textLocalizer.name + " | the total of text Localizers is " + m_registeredTextLocalizers.Count);
            int index = 0;
            while (index < m_registeredTextLocalizers.Count)
            {
                //Debug.Log(index);
                if (textLocalizer.GetInstanceID() == m_registeredTextLocalizers[index].GetInstanceID())
                {
                    textLocalizerToRemoveIndex = index;
                    break;
                }
                index++;
            }

            if (textLocalizerToRemoveIndex == -1)
            {
                Debug.Assert(!textLocalizer.gameObject.activeInHierarchy, "TextLocalizer to delete not found !");
                return;
            }
            m_registeredTextLocalizers.RemoveAt(textLocalizerToRemoveIndex);
        }

        public string Localize(string key)
        {
            string translation = "";
            if (m_currentLanguageData.LanguageDictionary.TryGetValue(key, out translation))
            {
                return translation;
            }
            else
            {
                Debug.LogWarning("No translation found for key : " + key);
                return key;
            }
        }

        public string Localize(string key, int languageIndex)
        {
            string translation = "";
            if (m_allLanguagesData.GetLanguageData(GetValidIndex(languageIndex)).LanguageDictionary.TryGetValue(key, out translation))
            {
                return translation;
            }
            else
            {
                Debug.LogWarning("No translation found for key : " + key);
                return key;
            }
        }

        private void UpdateLocalizers()
        {
            for (int i = 0; i < m_registeredTextLocalizers.Count; i++)
            {
                var textLocalizer = m_registeredTextLocalizers[i];
                textLocalizer.TextTarget.text = Localize(textLocalizer.Key);
                if (textLocalizer.Formatter != null)
                {
                    textLocalizer.Formatter.Invoke(textLocalizer.TextTarget.text, textLocalizer);
                    textLocalizer.UpdateComponent();
                }
            }
        }

        public void ForceUpdate()
        {
            UpdateLocalizers();
        }
        #endregion

        #region Utils
#if UNITY_EDITOR
        public void GenerateLocalizationData()
        {
            Debug.Log("Generate Languages Data");

            StreamReader reader = new StreamReader(path);
            string line = reader.ReadLine();
            int charIndex = 0;
            int numberOfLanguage = 0;
            while (charIndex < line.Length)
            {
                if (line[charIndex] == '\t')
                {
                    numberOfLanguage++;
                }
                charIndex++;
            }

            for(int i = 0; i < numberOfLanguage; i++)
            {
                LanguageData newLanguageData = ScriptableObject.CreateInstance<LanguageData>();
                newLanguageData.SetLanguageDictionary(ReadLanguage(i));
                EditorUtility.SetDirty(newLanguageData);
                string languageDataPath = "Assets/07_Data/Languages/LanguageData" + i + ".asset";
                AssetDatabase.CreateAsset(newLanguageData, languageDataPath);
                m_allLanguagesData.AddOrModifiyLanguageData(newLanguageData, i);
                EditorUtility.SetDirty(m_allLanguagesData);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
#endif

        public int GetValidIndex(int index)
        {
            int indexToReturn = 0;
            indexToReturn = index % m_allLanguagesData.NumberOfLanguages;
            if (indexToReturn < 0)
            {
                indexToReturn = m_allLanguagesData.NumberOfLanguages + indexToReturn;
            }

            return indexToReturn;
        }
        #endregion
        #endregion
    }

    [System.Serializable]
    internal class LanguageDictionary : SerializableDictionary<string, string>
    {
         
    }
}
