using UnityEngine;
namespace Coding.Language
{
    public class Int : Object<IVariableContainer>, IVariable<int>
    {
        public Int(string name, IVariableContainer container, object value, Type type = Type.Int) : base(name, container, type)
        {
        }

        public int Get()
        {
            return (int)value;
        }
        public void Set(int value)
        {
            this.value = value;
        }
    }
}
