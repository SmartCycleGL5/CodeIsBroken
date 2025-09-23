using UnityEngine;
namespace Coding.Language
{
    public class String : Variable, IGetSetAble<string>
    {
        public String(string name, IVariable container, object value, Type type = Type.String) : base(name, container, value, type)
        {
        }

        public string Get()
        {
            return (string)value;
        }
        public void Set(string value)
        {

        }
    }
}
