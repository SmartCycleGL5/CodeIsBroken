using SharpCube.Object;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SharpCube
{
    public static partial class Compiler
    {
        public readonly static Dictionary<string, IKeyword> Keywords = new()
        {
            { "class", new Encapsulation(Keyword.Class, Color.blue, Class.Create) },
            { "struct", new Encapsulation(Keyword.Struct, Color.blue, Class.Create) },

            //{ "void", new IKeyword("void", Color.blue) },
            //{ "int", new IKeyword("int", Color.blue) },
            //{ "float", new IKeyword("float", Color.blue) },
            //{ "string", new IKeyword("string", Color.blue) },
            //{ "bool", new IKeyword("bool", Color.blue) },

            //{ "if", new IKeyword("if", Color.blue) },
            //{ "else", new IKeyword("else", Color.blue) },
        };

        public static MachineCode toCompile;
        public static List<string> convertedCode;


        /// <summary>
        /// Creates Classes, variables & methods
        /// </summary>
        /// <param name="machineCode"></param>
        public static void Interporate(MachineCode machineCode)
        {
            toCompile = machineCode;

            Debug.Log("[Compiler] Compile");

            string rawCode = machineCode.Code;
            if (!ConvertCode(rawCode, out convertedCode)) return;

            for (int i = 0; i < convertedCode.Count; i++)
            {
                string word = convertedCode[i];

                if (Keywords.ContainsKey(word))
                {
                    Keywords[word].create.Invoke(i);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Compile()
        {

        }


        static bool ConvertCode(string raw, out List<string> convertedCode)
        {
            convertedCode = Regex.Split(raw, "( |\t|\n|;|}|{)").ToList();
            convertedCode.RemoveAll(x => x == "\t" || x == "\n" || x == " " || x == "");      

            return true;
        }
    }
}
