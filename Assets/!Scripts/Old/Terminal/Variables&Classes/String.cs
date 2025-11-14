using UnityEngine;
namespace Coding.SharpCube
{
    public class String : Variable, ISettable<string>, IGettable<string>
    {
        public String(string name, IVariableContainer container, string value = null, VariableType type = SharpCube.VariableType.String) : base(name, container, type)
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

        public override VariableType Type()
        {
            return SharpCube.VariableType.String;
        }
    }
}
