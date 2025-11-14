using UnityEngine;

namespace SharpCube.Type
{
    public class Int : IType<int>
    {
        public int Value { get; set; }
    }
}