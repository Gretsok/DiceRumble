
namespace MOtter.Context
{
    public class FloatContext : AContext
    {
        public float Value { get; internal set; } = 0f;
        public FloatContext(float value, string name) : base(name)
        {
            Value = value;
        }

        public FloatContext(float value, int nameHash) : base(nameHash)
        {
            Value = value;
        }
    }
}