using Coding.SharpCube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SharpCube
{
    public static partial class Compiler
    {
        public static Dictionary<string, Action> Keywords = new()
        {
            { "class", DefineClass },
        };

        public static void Compile(MachineCode machineCode)
        {
            CompileClass();

            void CompileClass()
            {
                Debug.Log("[Compiler] Compile");
                string rawCode = machineCode.Code;

                //string modified = rawCode.Replace("\n", "");
                //modified = modified.Replace("\t", "");

                //List<string> code = Regex.Split(modified, "(;|{|})").ToList();
                List<string> code = Regex.Split(rawCode, "( |\t|\n)").ToList();
                code.RemoveAll(x => x == "\t" || x == "\n" || x == " " || x == "");

                for (int i = 0; i < code.Count; i++)
                {
                    string word = code[i]; 
                    if (word == "class")
                    {
                        Debug.Log("[Compiler] Found " + word);
                        Keywords[word].Invoke();
                    }
                }

            }
        }
    }

    public static partial class Compiler
    {
    }
}
