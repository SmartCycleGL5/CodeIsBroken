using UnityEngine;
namespace Coding.Language
{
    public class Float : Variable, ISettable<float>, IGettable<float>
    {
        public Float(string name, IVariableContainer container, object value, Type type = Type.Float) : base(name, container, type)
        {
        }

        public float Get()
        {
            return (float)value;
        }
        public void Set(float value)
        {
            this.value = value;
        }
    }
}
