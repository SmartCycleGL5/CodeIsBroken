using UnityEngine;
namespace Coding.Language
{
    public class Float : Variable, IGetSetAble<float>
    {
        public Float(string name, IVariable container, object value, Type type = Type.Float) : base(name, container, value, type)
        {
        }

        public float Get()
        {
            return (float)value;
        }
        public void Set(float value)
        {

        }
    }
}
