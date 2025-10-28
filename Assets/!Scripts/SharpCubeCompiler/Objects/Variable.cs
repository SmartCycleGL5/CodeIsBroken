using SharpCube.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharpCube.Object
{
    [Serializable]
    public class Variable : IObject, IType
    {
        [field: SerializeField] public string name { get; set; }
        public Type type { get; set; }

        public object value { get; set; }

        public Variable(object value = default)
        {
            
        }

        public static void Create(Encapsulation encapsulation, List<string> context, int line, Properties properties)
        {
            encapsulation.variables.Add(context[line + 1], new Variable(), properties.privilege);
        }
    }
}