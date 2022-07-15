

using System;

namespace MOtter.Context
{
    public abstract class AContext
    {
        internal Action<AContext> OnChanges;
        internal int NameHash;
        public AContext(string name)
        {
            NameHash = MOtterApplication.GetInstance().UTILS.GetDeterministicHashCode(name);
        }

        public AContext(int hashName)
        {
            NameHash = hashName;
        }

        public void RegisterToContext(Action<AContext> callback)
        {
            OnChanges += callback;
        }


    }
}