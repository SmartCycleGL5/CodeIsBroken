using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SharpCube.keyword;
using UnityEngine;

namespace SharpCube
{
    public static partial class Compiler
    {
        public readonly static Dictionary<string, Keyword> Objects = new()
        {
            { "class", new Keyword("class", Color.blue) },
            { "struct", new Keyword("struct", Color.blue) },
        };
        public readonly static Dictionary<string, Keyword> Types = new()
        {
            { "void", new Keyword("void", Color.blue) },
            { "int", new Keyword("int", Color.blue) },
            { "float", new Keyword("float", Color.blue) },
            { "string", new Keyword("string", Color.blue) },
            { "bool", new Keyword("bool", Color.blue) },
        };
        public readonly static Dictionary<string, Keyword> SelectionStatements = new()
        {
            { "if", new Keyword("if", Color.blue) },
            { "else", new Keyword("else", Color.blue) },
        };

        public static void Compile(MachineCode machineCode)
        {
            Debug.Log("[Compiler] Compile");

            string rawCode = machineCode.Code;

            List<string> code = Regex.Split(rawCode, "( |\t|\n|;|}|{)").ToList();
            code.RemoveAll(x => x == "\t" || x == "\n" || x == " " || x == "");

            ConvertCode(code, out List<object> convetedCode);

            for (int i = 0; i < code.Count; i++)
            {
                string word = code[i];
                if (!Objects.ContainsKey(word)) continue;


                Debug.Log("[Compiler] Found " + word);
            }

            Debug.Log("[Compiler] Finished Compile");
        }


        static void ConvertCode(List<string> code, out List<object> convertedCode)
        {
            convertedCode = new List<object>();
            foreach (string line in code)
            {
                convertedCode.Add(line);
            }
        }
    }
}
