using UnityEngine;

namespace MOtter.LevelData
{
    public abstract class AdditionalDataInLevel : ScriptableObject
    {
        [SerializeField]
        private bool m_mustBeUnique = false;
        public bool MustBeUnique => m_mustBeUnique;
    }
}