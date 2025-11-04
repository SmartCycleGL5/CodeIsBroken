using SharpCube.Type;
using System;
using UnityEngine;
using Types = SharpCube.Type.Types;

namespace SharpCube
{
    [Serializable]
    public class Variable : IReference
    {
        [field: SerializeField] public string name { get; set; }
        public IContainer container { get; set; }

        Object obj;

        public Variable(string name, Properties properties, IContainer container, IType value)
        {
            this.name = name;
            this.container = container;
            
            if (value != null)
            {
                this.obj = new(value);   
            }

            container.containedVarialbes.Add(name, this, properties.privilege);
            Compiler.Keywords[KeywordType.Reference].Add(name, new Reference(name, Compiler.variableColor, this));
            Debug.Log($"[Variable] new variable: {name} of type {value}");
        }

        public void Set(IType newValue)
        {
            obj.value = newValue;
        }
        public IType Get()
        {
            return obj.value;
        }
        
        public static void Create(IContainer encapsulation, Line line, int initializer, Properties properties)
        {
            string name = line.sections[initializer + 1];
            string type = line.sections[initializer];
            
            try
            {
                Types T = Enum.Parse<Types>(type, true);
                new Variable(name, properties, encapsulation, IType.NewType(T));
            }
            catch
            {
                new Variable(name, properties, encapsulation, null);
            }
        }
    }
}