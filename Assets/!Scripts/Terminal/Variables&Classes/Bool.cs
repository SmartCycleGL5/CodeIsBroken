using Coding.Language;
using UnityEngine;
namespace Coding.Language
{
    public class Bool : Variable, IGetSetAble<bool>
    {
        public Bool(string name, IVariable container, object value, Type type = Type.Bool) : base(name, container, value, type)
        {
        }

        public bool Get()
        {
            return (bool)value;
        }
        public void Set(bool value)
        {
            
        }
    }
}
