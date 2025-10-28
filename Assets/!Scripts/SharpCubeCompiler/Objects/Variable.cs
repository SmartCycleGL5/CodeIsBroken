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
        //public Type type { get; set; }

        public object value { get; set; }

        public Variable(string name/*, Type type*/, Properties properties, Encapsulation encapsulation, object value = default)
        {
            encapsulation.variables.Add(name, this, properties.privilege);
        }

        public void Set(object newValue)
        {
            if (value.GetType() != newValue.GetType())
                PlayerConsole.LogError($"Cannot implicitly convert type '{newValue.GetType()}' to '{value.GetType()}'");

            Debug.Log(newValue);
        }
    }
}