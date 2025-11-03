using UnityEngine;

namespace SharpCube.Type
{
    public class String : IType<string>
    {
        public string Value { get; set; }
    }
}