using Coding.Language;
using UnityEngine;
namespace Coding.Language
{
    public class Bool : Variable, ISettable<bool>, IGettable<bool>
    {
        public Bool(string name, IVariableContainer container, object value, Type type = Type.Bool) : base(name, container, type)
        {
        }

        public bool Get()
        {
            return (bool)value;
        }
        public void Set(bool value)
        {
            this.value = value;
        }
    }
}
