using System.Collections.Generic;
using UnityEngine;

namespace MOtter.StatesMachine
{
    public class FlowState : StateMonoBehaviour
    {
        [SerializeField] private Panel[] m_panels = null;

        public override void EnterState()
        {
            base.EnterState();
            DisplayPanels();
            RegisterReferences();
            SetUpDependencies();
            RegisterEvents();
        }

        internal void DisplayPanels()
        {
            for (int i = 0; i < m_panels.Length; i++)
            {
                m_panels[i].Show();
            }
        }

        internal virtual void RegisterEvents()
        {

        }

        internal virtual void SetUpDependencies()
        {

        }

        internal virtual void RegisterReferences()
        {

        }

        public T GetPanel<T>() where T : Panel
        {
            for(int i = 0; i < m_panels.Length; ++i)
            {
                if(m_panels[i] is T)
                {
                    return m_panels[i] as T;
                }
            }
            return null;
        }

        public List<T> GetPanels<T>() where T : Panel
        {
            List<T> panelsToReturn = new List<T>();
            for(int i = 0; i < m_panels.Length; ++i)
            {
                if (m_panels[i] is T)
                {
                    panelsToReturn.Add(m_panels[i] as T);
                }
            }
            return panelsToReturn;
        }

        internal virtual void UnregisterEvents()
        {

        }

        internal virtual void CleanUpDependencies()
        {

        }

        internal void HidePanels()
        {
            for (int i = 0; i < m_panels.Length; i++)
            {
                m_panels[i].Hide();
            }
        }

        public override void ExitState()
        {
            UnregisterEvents();
            CleanUpDependencies();
            HidePanels();
            base.ExitState();
        }
    }
}