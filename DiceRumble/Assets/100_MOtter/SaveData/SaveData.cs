using UnityEngine;

namespace MOtter.Save
{
    [System.Serializable]
    public class SaveData
    {
        public string SaveName = string.Empty;


        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string a_Json)
        {
            JsonUtility.FromJsonOverwrite(a_Json, this);


        }
    }
}