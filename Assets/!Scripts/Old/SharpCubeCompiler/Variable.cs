using ScriptEditor.Console;
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

            //container.containedVarialbes.Add(name, this, properties.privilege);
            container.keywords.Add(Keywords.Type.Reference, new Reference<Variable>(name, this));
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
        
        public static void Create(IContainer container, Line line, int initializer, Properties properties)
        {
            //if(container == null) //PlayerConsole.LogError("Variables must be defined within a class or method");
            
            //string name = line.sections[initializer + 1];
            //string type = line.sections[initializer];
            
            //Types T = Enum.Parse<Types>(type, true);
            //new Variable(name, properties, container, IType.NewType(T));
        }
    }
}