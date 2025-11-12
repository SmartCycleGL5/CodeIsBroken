using UnityEngine;

namespace SharpCube.Type
{
    public class Bool : IType<bool>
    {
        public bool Value { get; set; }

        public bool GetValue()
        {
            return Value;
        }
    }
}