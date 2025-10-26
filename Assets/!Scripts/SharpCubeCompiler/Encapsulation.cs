using System.Collections.Generic;
using UnityEngine;
using SharpCube.Object;

namespace SharpCube
{
    [System.Serializable]
    public class Encapsulation : IContainer
    {
        [field: SerializeField] public Memory<Variable> variables { get; set; } = new Memory<Variable>();
        
        [field: SerializeField] public List<string> content { get; set; } = new List<string>();

        public Encapsulation(int location)
        {
            location += 2;

            for (int i = location + 1; i < FindEndOfEndEncapsulation(location); i++)
            {
                content.Add(Compiler.convertedCode[i]);
            }
            
            Compiler.Compile(content);
        }

        public static int FindEndOfEndEncapsulation(int startEncapsulation)
        {
            string start = Compiler.Keywords[KeywordType.Valid]["{"].key;
            string end = Compiler.Keywords[KeywordType.Valid]["}"].key;

            if (Compiler.convertedCode[startEncapsulation] != start)
            {
                PlayerConsole.LogError("{ expected");
                PlayerConsole.LogError("} expected");
                return -1;
            }
            
            startEncapsulation++;
            int encapsulations = 1;

            for (int i = startEncapsulation; i < Compiler.convertedCode.Count; i++)
            {
                string current = Compiler.convertedCode[i];
                
                if (current == start)
                {
                    encapsulations++;
                    continue;
                }
                if (current == end)
                {
                    encapsulations--;
                }

                if (encapsulations == 0)
                {
                    return i;
                }
            }
            
            return -1;
        }
    }   
}
