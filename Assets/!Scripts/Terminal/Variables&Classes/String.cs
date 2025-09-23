using UnityEngine;
namespace Coding.Language
{
    public class String : Object<IVariableContainer>, IVariable<string>
    {
        public String(string name, IVariableContainer container, object value, Type type = Type.String) : base(name, container, type)
        {
        }

        public string Get()
        {
            return (string)value;
        }
        public void Set(string value)
        {
            this.value = value;
        }
    }
}
