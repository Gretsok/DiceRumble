using MOtter.StatesMachine;
using UnityEngine;

namespace DR
{
    public class DRGameManager : GameManager
    {
        [Header("DR Specific Refs")]
        [SerializeField]
        private GlobalGameData m_globalGameData = null;
        public GlobalGameData GlobalGameData => m_globalGameData;
    }
}