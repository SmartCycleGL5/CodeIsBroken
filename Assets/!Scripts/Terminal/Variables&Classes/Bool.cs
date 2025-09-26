using Coding.Language;
using UnityEngine;
namespace Coding.Language
{
    public class Bool : Variable, ISettable<bool>, IGettable<bool>
    {
        public Bool(string name, IVariableContainer container, bool value = false, Type type = Language.Type.Bool) : base(name, container, type)
        {
            Set(value);
        }

        public bool Get()
        {
            return (bool)info.value;
        }
        public void Set(bool value)
        {
            info.value = value;
        }

        public override Type Type()
        {
            return Language.Type.Bool;
        }
    }
}
