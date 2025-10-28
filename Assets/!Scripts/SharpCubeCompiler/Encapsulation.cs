using System.Collections.Generic;
using UnityEngine;
using SharpCube.Object;

namespace SharpCube
{
    [System.Serializable]
    public class Encapsulation
    {
        [field: SerializeField] public Memory<Variable> variables { get; set; } = new Memory<Variable>();
        
        [field: SerializeField] public List<string> content { get; set; } = new List<string>();

        public Encapsulation(int location, List<string> context)
        {
            location += 2;
            int end = FindEndOfEndEncapsulation(location, context);
            
            if(end == -1) throw new System.Exception("couldn't get encapsulation");
            

            for (int i = location; i <= end; i++)
            {
                Debug.Log(context[i]);
                content.Add(context[i]);
            }
            
            content.RemoveAt(0);
            content.RemoveAt(content.Count - 1);
            
            if(content.Count > 0)
                Compiler.Compile(content, this);
        }

        public static int FindEndOfEndEncapsulation(int startEncapsulation, List<string> context)
        {
            string start = Compiler.Keywords[KeywordType.Valid]["{"].key;
            string end = Compiler.Keywords[KeywordType.Valid]["}"].key;

            if (context[startEncapsulation] != start)
            {
                PlayerConsole.LogError("{ expected");
                PlayerConsole.LogError("} expected");
                return -1;
            }
            
            startEncapsulation++;
            int encapsulations = 1;

            for (int i = startEncapsulation; i < context.Count; i++)
            {
                string current = context[i];
                
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
            
            PlayerConsole.LogError("} expected");
            return -1;
        }
    }   
}
