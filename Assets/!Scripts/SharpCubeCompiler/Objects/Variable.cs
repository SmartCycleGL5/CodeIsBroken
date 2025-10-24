using SharpCube.Object;
using System;
using UnityEngine;

namespace SharpCube.Object
{
    [Serializable]
    public class Variable : IObject, IType
    {
        [field: SerializeField] public string name { get; set; }
        public Type type { get; set; }

        public object value { get; set; }

        public static void Create(int line, Properties properties)
        {
            new Variable();
        }
    }
}