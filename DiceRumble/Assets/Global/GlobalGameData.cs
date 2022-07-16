using UnityEngine;

namespace DR
{
    [CreateAssetMenu(fileName = "GlobalGameData", menuName = "Dice Rumble/Global/GlobalGameData")]
    public class GlobalGameData : ScriptableObject
    {
        [System.Serializable]
        public struct TeamData
        {
            public Color TeamColor;
        }
        [SerializeField]
        private TeamData m_firstTeamData = default;
        [SerializeField]
        private TeamData m_secondTeamData = default;

        public TeamData FirstTeamData => m_firstTeamData;
        public TeamData SecondTeamData => m_secondTeamData;
    }
}