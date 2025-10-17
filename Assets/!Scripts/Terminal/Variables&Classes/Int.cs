using UnityEngine;
namespace Coding.SharpCube
{
    public class Int : Variable, ISettable<int>, IGettable<int>
    {
        public Int(string name, IVariableContainer container, int value = 0, Type type = SharpCube.Type.Int) : base(name, container, type)
        {
            Set(value);
        }

        public int Get()
        {
            return (int)info.value;
        }
        public void Set(int value)
        {
            info.value = value;
        }

        public override Type Type()
        {
            return SharpCube.Type.Int;
        }
    }
}
