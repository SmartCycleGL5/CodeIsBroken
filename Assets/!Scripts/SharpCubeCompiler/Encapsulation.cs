using System.Collections.Generic;
using UnityEngine;
using SharpCube.Object;
using System.Linq;

namespace SharpCube
{
    [System.Serializable]
    public class Encapsulation
    {
        [field: SerializeField] public Memory<Variable> variables { get; set; } = new Memory<Variable>();
        
        [field: SerializeField] public List<Line> content { get; set; } = new();

        public Encapsulation(int line)
        {
            int end = FindEndOfEndEncapsulation(line, Compiler.currentContext);

            for (int i = line + 1; i < end - 1; i++)
            {
                content.Add(Compiler.currentContext[i]);
            }
            
            if(content.Count > 0)
                Compiler.Compile(content, this);
        }

        public static int FindEndOfEndEncapsulation(int startEncapsulation, List<Line> context)
        {
            string start = Compiler.Keywords[KeywordType.Valid]["{"].key;
            string end = Compiler.Keywords[KeywordType.Valid]["}"].key;

            if (context[startEncapsulation].sections[0] != start)
            {
                Debug.Log(context[startEncapsulation].sections[0]);
                PlayerConsole.LogError("{ expected");
                return -1;
            }
            
            startEncapsulation++;
            int encapsulations = 1;

            for (int i = startEncapsulation; i < context.Count; i++)
            {
                if (context[i].sections.Contains(start))
                {
                    encapsulations++;
                    continue;
                }
                if (context[i].sections.Contains(end))
                {
                    encapsulations--;
                }

                if (encapsulations == 0)
                {
                    return i;
                }
            }
            
            PlayerConsole.LogError("} expected");
            return -1;
        }
    }   
}
