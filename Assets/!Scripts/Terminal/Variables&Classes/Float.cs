using UnityEngine;
namespace Coding.SharpCube
{
    public class Float : Variable, ISettable<float>, IGettable<float>
    {
        public Float(string name, IVariableContainer container, float value = 0f, Type type = SharpCube.Type.Float) : base(name, container, type)
        {
            Set(value);
        }

        public float Get()
        {
            return (float)info.value;
        }
        public void Set(float value)
        {
            info.value = value;
        }

        public override Type Type()
        {
            return SharpCube.Type.Float;
        }
    }
}
