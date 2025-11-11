using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharpCube
{
    [Serializable]
    public class Class : IReference, IContainer
    {
        public string name { get; set; }
        public Class inherit { get; set; }
        public IContainer container { get; set; }
        
        [field:SerializeField]public Keywords keywords { get; set; } = Keywords.Default;
        public Keywords allKeywords => container == null ? keywords : Keywords.Combine(keywords, container.allKeywords);
        
        [field:SerializeField] public Encapsulation encapsulation { get; set; }
        
        private Line line;

        public Class(IContainer container,string name, Properties properties, Line line)
        {
            this.name = name;
            this.line = line;

            if(container != null) 
                this.container = container;
            
            Compiler.containersToCompile.Add(this);
            
            if (container == null)
            {
                Compiler.UniversalKeywords.Add(Keywords.Type.Initializer, new Initializer(name, Variable.Create));
                //Compiler.UniversalKeywords.keys[Keywords.Type.Reference].Add(name, new Reference<Class>(name, this)); we deal with it later mann
                Compiler.toCompile.classes.Add(name, this, properties.privilege);
            }
            else
            {
                container.keywords.Add(Keywords.Type.Initializer, new Initializer(name, Variable.Create));
                //container.additionalKeywords.keys[Keywords.Type.Reference].Add(name, new Reference<Class>(name, this));
            }
            
            Debug.Log($"[Class] new class: {name}");
        }

        public static void Create(IContainer container, Line line, int initializer, Properties properties)
        {
            string name = line.sections[line.sections.Length - 1];

            if (container == null)
            {
                if(Compiler.UniversalKeywords.Contains(name))
                    PlayerConsole.LogError($"The class '{name}' is already used in the current context");   
            }
            else 
            {
                if(container.keywords.Contains(name))
                    PlayerConsole.LogError($"The class '{name}' is already used in the current context");   
            }


            new Class(container, name, properties, line);
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
