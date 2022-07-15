using System.Collections.Generic;
using UnityEngine;

namespace MOtter.DataSceneConveyance
{
    public class DataSceneConveyor
    {
        private List<SceneConveyanceDataContainer> m_dataContainers = new List<SceneConveyanceDataContainer>();
        public void RegisterContainer(SceneConveyanceDataContainer p_dataContainer)
        {
            if(!m_dataContainers.Contains(p_dataContainer))
            {
                m_dataContainers.Add(p_dataContainer);
            }
            else
            {
                Debug.LogError($"{p_dataContainer.ToString()} is already registered as a data container.");
            }
        }

        public void UnregisterContainer(SceneConveyanceDataContainer p_dataContainer)
        {
            if (m_dataContainers.Contains(p_dataContainer))
            {
                m_dataContainers.Remove(p_dataContainer);
            }
            else
            {
                if (p_dataContainer != null)
                    Debug.LogError($"{p_dataContainer.ToString()} is not registered as a data container.");
                else
                    Debug.LogError("Trying to unregister a null container.");
            }
        }

        public T GetFirstContainer<T>() where T : SceneConveyanceDataContainer
        {
            for(int i = 0; i < m_dataContainers.Count; ++i)
            {
                if(m_dataContainers[i] is T)
                {
                    return m_dataContainers[i] as T;
                }
            }
            
            return null;
        }

        public List<T> GetContainers<T>() where T : SceneConveyanceDataContainer
        {
            List<T> containersToReturn = new List<T>();
            for (int i = 0; i < m_dataContainers.Count; ++i)
            {
                if (m_dataContainers[i] is T)
                {
                    containersToReturn.Add(m_dataContainers[i] as T);
                }
            }
            return containersToReturn;
        }
    }
}