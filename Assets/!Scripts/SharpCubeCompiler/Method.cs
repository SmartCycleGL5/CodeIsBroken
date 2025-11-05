using SharpCube.Type;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharpCube
{
    public class Method : IReference, IContainer
    {
        public string name { get; set; }
        public IContainer container { get; set; }
        
        public Encapsulation encapsulation { get; set; }
        public Keywords additionalKeywords { get; set; }

        private Line line;

        public Method(IContainer container, string name, object[] args, Properties properties, Line line)
        {
            this.name = name;
            this.line = line;
            this.container = container;
            
            Compiler.containersToCompile.Add(this);
            
            container.additionalKeywords.keys[Keywords.Type.Initializer].Add(name, new Initializer(name, Variable.Create));
            
            Debug.Log($"[Method] new method: {name}");
        }

        public static void Create(IContainer container, Line line, int initializer, Properties properties)
        {
            PlayerConsole.LogWarning($"Methods not implemented: {line.GetLine()}");
            
            if(container == null) PlayerConsole.LogError("Methods must be defined within a class or method");
            
            string name = line.sections[initializer + 1];
            string type = line.sections[initializer];

            new Method(container, name, null, properties, line);
        }
        public void StartCompile()
        {
            List<Line> context;

            if (container != null)
                context = container.encapsulation.content;
            else
                context = Compiler.convertedCode;

            int start = context.IndexOf(line) + 1;
            
            this.encapsulation = new Encapsulation(this, context, start);
        }
    }

}