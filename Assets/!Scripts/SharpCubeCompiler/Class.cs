using System;
using System.Collections.Generic;
using SharpCube.Type;
using UnityEngine;

namespace SharpCube
{
    [Serializable]
    public class Class : IReference, IContainer
    {
        public string name { get; set; }
        public Memory<Variable> containedVarialbes { get; set; } = new();
        [field:SerializeField] public Encapsulation encapsulation { get; set; }

        public static Memory<Class> initializedClasses { get; private set; } = new();
        private Line line;


        public Class(string name, Properties properties, Line line)
        {
            this.name = name;
            this.line = line;
            
            initializedClasses.Add(name, this, properties.privilege);
            
            Compiler.toCompile.classes.Add(name, this, properties.privilege);
            Compiler.containersToCompile.Add(this);
            Compiler.Keywords[KeywordType.Initializer].Add(name, new Initializer(name, Compiler.classColor, Variable.Create));
            
            Debug.Log($"[Class] new class: {name}");
        }

        public static void Create(IContainer encapsulation, Line line, int initializer, Properties properties)
        {
            string name = line.sections[line.sections.Length - 1];

            if (initializedClasses.Contains(name))
                PlayerConsole.LogError($"The class {name} is already in use");


            new Class(name, properties, line);
        }

        public static void ClearClasses()
        {
            initializedClasses.Clear();
        }

        public void StartCompile()
        {
            this.encapsulation = new Encapsulation(Compiler.currentContext.IndexOf(line) + 1);
        }
    }
}
