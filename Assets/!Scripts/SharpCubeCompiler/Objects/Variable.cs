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

        public Var value;

        public Variable(string name, Type type, Properties properties, Encapsulation encapsulation, Var value = null)
        {
            this.name = name;
            value.type = type;

            switch(type)
            {
                case Type.Float:
                    Set(new Var(Type.Float, 0));
                    break;
                case Type.Int:
                    Set(new Var(Type.Int, 0));
                    break;
                case Type.String:
                    Set(new Var(Type.String, ""));
                    break;
                case Type.Bool:
                    Set(new Var(Type.Bool, false));
                    break;
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