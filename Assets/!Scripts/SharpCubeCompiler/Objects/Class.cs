using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharpCube.Object
{
    [Serializable]
    public class Class : IObject
    {
        class whatever
        {
            
        }
        [field:SerializeField] public string name { get; set; }
        [field:SerializeField] public Encapsulation Encapsulation { get; set; }

        public static Memory<Class> initializedClasses { get; private set; } = new();


        public Class(string name, Properties properties, Encapsulation encapsulation)
        {
            Debug.Log($"[Class] new class {name}");

            this.name = name;
            this.Encapsulation = encapsulation;

            initializedClasses.Add(name, this, properties.privilege);
            Compiler.toCompile.classes.Add(name, this, properties.privilege);
        }

        public static void Create(Encapsulation encapsulation, List<string> context, int line, Properties properties)
        {
            if (encapsulation != null)
            {
                PlayerConsole.LogError($"Cannot create a class within another class");
                throw new Exception("Player error");
            }
            string name = context[line + 1];

            if (initializedClasses.Contains(name))
            {
                PlayerConsole.LogError($"The class {name} is already in use");
                throw new Exception("Player error");
            }

            new Class(name, properties, new Encapsulation(line, context));
        }

        public static void ClearClasses()
        {
            initializedClasses.Clear();
        }
    }
}
