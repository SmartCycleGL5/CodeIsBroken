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
            return (string)info.value;
        }
        public void Set(string value)
        {
            info.value = value;
        }

        public override Type Type()
        {
            return Language.Type.String;
        }
    }
}
