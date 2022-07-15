using System.Collections.Generic;
using UnityEngine;

namespace MOtter.Context
{
    public class ContextManager : MonoBehaviour
    {
        public Dictionary<int, AContext> RegisteredContext { get; private set; } = new Dictionary<int, AContext>();

        #region accessors
        #region contextCreation
        public void RegisterNewContext(AContext context)
        {
            RegisteredContext.Add(context.NameHash, context);
        }
        #endregion

        #region Getters
        public T GetContext<T>(string name) where T : AContext
        {
            return GetContext<T>(MOtterApplication.GetInstance().UTILS.GetDeterministicHashCode(name));
        }
        public T GetContext<T>(int hashName) where T : AContext
        {
            AContext contextToReturn = null;
            RegisteredContext.TryGetValue(hashName, out contextToReturn);
            return (T) contextToReturn;
        }
        #endregion

        #region Deleters
        public bool DeleteContext(string name)
        {
            return DeleteContext(MOtterApplication.GetInstance().UTILS.GetDeterministicHashCode(name));
        }
        public bool DeleteContext(int hashName)
        {
            if(RegisteredContext.ContainsKey(hashName))
            {
                RegisteredContext.Remove(hashName);
                return true;
            }
            return false;
        }
        #endregion
        #endregion

        
        
    }
}