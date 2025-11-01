using System;
using UnityEngine;

namespace SharpCube
{
    [Serializable]
    public class Variable : IReference
    {
        [field: SerializeField] public string name { get; set; }

        [HideInInspector] Object value;

        public Variable(string name, Properties properties, Encapsulation encapsulation, IReference value = null)
        {
            this.name = name;

            this.value = new();
            if (value != null)
            {
                this.value.Set(value);
            }

            encapsulation.containedVarialbes.Add(name, this, properties.privilege);
        }
    }
}