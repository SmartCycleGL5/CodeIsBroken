using UnityEngine;
namespace Coding.Language
{
    public class Float : Variable, ISettable<float>, IGettable<float>
    {
        public Float(string name, IVariableContainer container, float value = 0f, Type type = Language.Type.Float) : base(name, container, type)
        {
            Set(value);
        }

        public float Get()
        {
            return (float)value;
        }
        public void Set(float value)
        {
            this.value = value;
        }

        public override Type Type()
        {
            return Language.Type.Float;
        }
    }
}
