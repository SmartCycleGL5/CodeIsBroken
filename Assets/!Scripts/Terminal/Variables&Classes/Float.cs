using UnityEngine;
namespace Coding.Language
{
    public class Float : Variable
    {
        public Float(string name, object value) : base(name, value)
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
