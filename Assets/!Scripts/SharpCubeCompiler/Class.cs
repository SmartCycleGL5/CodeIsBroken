using System;
using System.Collections.Generic;
using SharpCube.Type;
using UnityEngine;

namespace SharpCube
{
    [Serializable]
    public class Class : IReference
    {
        public string name { get; set; }
        [field:SerializeField] public Encapsulation encapsulation { get; set; }

        public static Memory<Class> initializedClasses { get; private set; } = new();


        public Class(string name, Properties properties, Encapsulation encapsulation)
        {
            this.encapsulation = encapsulation;

            initializedClasses.Add(name, this, properties.privilege);
            Compiler.toCompile.classes.Add(name, this, properties.privilege);
        }

        public static void Create(Encapsulation encapsulation, Line line, Properties properties)
        {
            string name = line.sections[line.sections.Length - 1];

            if (initializedClasses.Contains(name))
                PlayerConsole.LogError($"The class {name} is already in use");


            new Class(name, properties, new Encapsulation(Compiler.currentContext.IndexOf(line) + 1));
        }

        public static void ClearClasses()
        {
            initializedClasses.Clear();
        }
    }
}
