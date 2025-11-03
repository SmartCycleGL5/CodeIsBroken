using SharpCube.Type;
using System;
using UnityEngine;

namespace SharpCube
{
    [Serializable]
    public class Variable : IReference
    {
        [field: SerializeField] public string name { get; set; }

        [HideInInspector] Object obj;

        public Variable(string name, Properties properties, Encapsulation encapsulation, IType value)
        {
            this.name = name;
            this.obj = new(value);

            encapsulation.containedVarialbes.Add(name, this, properties.privilege);
        }

        public void Set(IType newValue)
        {
            obj.value = newValue;
        }
        public IType Get()
        {
            return obj.value;
        }
    }
}