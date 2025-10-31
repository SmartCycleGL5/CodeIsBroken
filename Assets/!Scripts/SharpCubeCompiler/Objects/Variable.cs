using SharpCube.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharpCube.Object
{
    [Serializable]
    public class Variable : IObject
    {
        [field: SerializeField] public string name { get; set; }

        [HideInInspector]public Var value;

        public Variable(string name, Type type, Properties properties, Encapsulation encapsulation, Var value = null)
        {
            if (type == Type.Void) PlayerConsole.LogError($"Field cannot have {type} type");
            Debug.Log($"{name}, {type}, {properties}, {encapsulation}, {value}");

            this.name = name;

            this.value = new(type);
            if(value != null)
            {
                Set(value);
            }

            encapsulation.variables.Add(name, this, properties.privilege);
        }

        public void Set(Var newValue)
        {
            if (newValue.type != value.type)
                PlayerConsole.LogError($"Cannot implicitly convert type '{newValue.GetType()}' to '{value.GetType()}'");

            value.value = newValue.value;
        }
    }
}