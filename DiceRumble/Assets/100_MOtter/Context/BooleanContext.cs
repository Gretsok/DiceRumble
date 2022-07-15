

namespace MOtter.Context
{
    public class BooleanContext : AContext
    {
        private bool m_value = false;
        public bool Value 
        { 
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
                OnChanges?.Invoke(this);
            }
        }

        public BooleanContext(bool value, string name) : base(name)
        {
            Value = value;
        }

        public BooleanContext(bool value, int nameHash) : base(nameHash)
        {
            Value = value;
        }

    }
}