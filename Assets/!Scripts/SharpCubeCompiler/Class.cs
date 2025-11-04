using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharpCube
{
    [Serializable]
    public class Class : IReference, IContainer
    {
        public string name { get; set; }
        public IContainer container { get; set; }

        public Memory<IReference> containedVarialbes { get; set; } = new();
        [field:SerializeField] public Encapsulation encapsulation { get; set; }

        public static Dictionary<string, Class> initializedClasses { get; private set; } = new();
        private Line line;


        public Class(IContainer container,string name, Properties properties, Line line)
        {
            this.name = name;
            this.line = line;

            if(container != null) 
                this.container = container;
            
            initializedClasses.Add(name, this);
            
            Compiler.toCompile.classes.Add(name, this, properties.privilege);
            Compiler.containersToCompile.Add(this);
            Compiler.Keywords[KeywordType.Initializer].Add(name, new Initializer(name, Compiler.classColor, Variable.Create));
            
            Debug.Log($"[Class] new class: {name}");
        }

        public static void Create(IContainer container, Line line, int initializer, Properties properties)
        {
            string name = line.sections[line.sections.Length - 1];

            if (initializedClasses.ContainsKey(name))
                PlayerConsole.LogError($"The class '{name}' is already in use");


            new Class(container, name, properties, line);
        }

        public static void ClearClasses()
        {
            initializedClasses.Clear();
        }

        public void StartCompile()
        {
            List<Line> context;

            if (container != null)
                context = container.encapsulation.content;
            else
                context = Compiler.convertedCode;

            int start = context.IndexOf(line) + 1;

            Debug.Log("[Class] " + context.IndexOf(line));
            this.encapsulation = new Encapsulation(context, start);
        }
    }
}
