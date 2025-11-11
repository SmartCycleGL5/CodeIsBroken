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
        public Keywords keywords { get; set; } = Keywords.Default;
        public Keywords allKeywords => Keywords.Combine(keywords, container.allKeywords );

        private Line line;

        public Method(IContainer container, string name, object[] args, Properties properties, Line line)
        {
            this.name = name;
            this.line = line;
            this.container = container;
            
            Compiler.containersToCompile.Add(this);
            
            container.keywords.Add(Keywords.Type.Reference, new Reference<Method>(name, this));
            
            Debug.Log($"[Method] new method: {name}");
        }

        public static void Create(IContainer container, Line line, int initializer, Properties properties)
        {
            if(container == null) PlayerConsole.LogError("Methods must be defined within a class or method");
            
            string name = line.sections[initializer + 1];
            string type = line.sections[initializer];
            
            if(container.allKeywords.Contains(name)) PlayerConsole.LogError($"Type already defines a member called '{name}'");

            new Method(container, name, null, properties, line);
        }
        
        public void StartCompile()
        {
            List<Line> context;

            context = container.encapsulation.content;

            int start = context.IndexOf(line) + 1;
            
            this.encapsulation = new Encapsulation(this, context, start);
        }
        
    }

}