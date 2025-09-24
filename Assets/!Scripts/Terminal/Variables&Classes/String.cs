using UnityEngine;
namespace Coding.Language
{
    public class String : Variable, ISettable<string>, IGettable<string>
    {
        public String(string name, IVariableContainer container, string value = null, Type type = Language.Type.String) : base(name, container, type)
        {
            Set(value);
        }

        public string Get()
        {
            return (string)value;
        }
        public void Set(string value)
        {
            this.value = value;
        }

        public override Type Type()
        {
            return Language.Type.String;
        }
    }
}
