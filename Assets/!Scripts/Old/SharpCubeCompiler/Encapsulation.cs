using ScriptEditor.Console;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SharpCube
{
    [System.Serializable]
    public class Encapsulation
    {
        public IContainer container { get; private set; }

        [field: SerializeField] public List<Line> content { get; set; } = new();

        public Encapsulation(IContainer container, List<Line> context, int line)
        {
            this.container = container;
            //Debug.Log("[Encapsulation] for " + container.GetType());
            int end = FindEndOfEndEncapsulation(line, context);

            for (int i = line + 1; i < end - 1; i++)
            {
                content.Add(context[i]);
            }
            
            Compiler.Compile(content, container);
        }

        public static int FindEndOfEndEncapsulation(int startEncapsulation, List<Line> context)
        {
            //string start = Compiler.UniversalKeywords.keys[Keywords.Type.Valid]["{"].name;
            //string end = Compiler.UniversalKeywords.keys[Keywords.Type.Valid]["}"].name;

            //if (context[startEncapsulation].sections[0] != start)
            //{
            //    PlayerConsole.LogError("{ expected");
            //    return -1;
            //}

            //startEncapsulation++;
            //int encapsulations = 1;

            //for (int i = startEncapsulation; i < context.Count; i++)
            //{
            //    if (context[i].sections.Contains(start))
            //    {
            //        encapsulations++;
            //        continue;
            //    }
            //    if (context[i].sections.Contains(end))
            //    {
            //        encapsulations--;
            //    }

            //    if (encapsulations == 0)
            //    {
            //        return i;
            //    }
            //}

            //PlayerConsole.LogError("} expected");
            return -1;
        }
    }
}
