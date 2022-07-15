
namespace MOtter.Context
{
    public class StatedContext : IntContext
    {
        public StatedContext(int value, string name) : base(value, name)
        {
        }

        public StatedContext(int value, int nameHash) : base(value, nameHash)
        {
        }
    }
}