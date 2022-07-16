using System.Collections.Generic;
using UnityEngine;

namespace DR.Gameplay.Level.Grid
{
    public class TileSelectorController : MonoBehaviour
    {
        [SerializeField]
        private List<Renderer> m_renderers = null;

        [SerializeField]
        private Material m_pathPossibleMat = null;
        [SerializeField]
        private Material m_teamDiceMat = null;
        [SerializeField]
        private Material m_selectedDiceMat = null;

        public void SetUpAsPathPossible()
        {
            m_renderers.ForEach(x => x.material = m_pathPossibleMat);
        }

        public void SetUpAsTeamDice()
        {
            m_renderers.ForEach(x => x.material = m_teamDiceMat);
        }

        public void SetUpAsSelectedDice()
        {
            m_renderers.ForEach(x => x.material = m_selectedDiceMat);
        }
    }
}