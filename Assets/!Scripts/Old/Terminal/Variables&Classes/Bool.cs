using Coding.SharpCube;
using UnityEngine;
namespace Coding.SharpCube
{
    public class Bool : Variable, ISettable<bool>, IGettable<bool>
    {
        public Bool(string name, IVariableContainer container, bool value = false, VariableType type = SharpCube.VariableType.Bool) : base(name, container, type)
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

        public override VariableType Type()
        {
            return SharpCube.VariableType.Bool;
        }
    }
}
