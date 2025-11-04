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

        public Encapsulation(List<Line> context, int line)
        {
            int end = FindEndOfEndEncapsulation(line, context);

            for (int i = line + 1; i < end - 1; i++)
            {
                content.Add(context[i]);
            }

            if (content.Count > 0)
                Compiler.Compile(content, container);
        }

        public static int FindEndOfEndEncapsulation(int startEncapsulation, List<Line> context)
        {
            string start = Compiler.Keywords[KeywordType.Valid]["{"].key;
            string end = Compiler.Keywords[KeywordType.Valid]["}"].key;

            if (context[startEncapsulation].sections[0] != start)
            {
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
