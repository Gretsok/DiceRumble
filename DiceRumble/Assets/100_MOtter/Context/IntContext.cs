
namespace MOtter.Context
{
    public class IntContext : AContext
    {
        public int Value { get; internal set; } = 0;

        public IntContext(int value, string name) : base(name)
        {
            Value = value;
        }

        public IntContext(int value, int nameHash) : base(nameHash)
        {
            Value = value;
        }
    }
}