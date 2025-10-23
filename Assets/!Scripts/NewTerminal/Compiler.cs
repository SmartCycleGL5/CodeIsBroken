using SharpCube.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SharpCube
{
    public enum KeywordType
    {
        Object,
        Accessibility,
        Type
    }
    public static partial class Compiler
    {
        public readonly static Dictionary<KeywordType, Dictionary<string, IKeyword>> Keywords = new()
        {
            { KeywordType.Object, new()
            {
                { "class", new Encapsulation("class", Color.blue, Class.Create) },
                { "struct", new Encapsulation("struct", Color.blue, Class.Create) },
            } },

            { KeywordType.Accessibility, new()
            {
                { "private", new Accessibility("private", Color.aliceBlue, Privilege.Private) },
                { "public",  new Accessibility("public", Color.aliceBlue, Privilege.Public) },
            } },

            { KeywordType.Type, new()
            {
                { "bool", new Type<bool>() },
                { "int", new Type<int>() },
                { "float", new Type<float>() },
                { "string", new Type<string>() },
            } },
        };

        public static Script toCompile;
        public static List<string> convertedCode;

        /// <summary>
        /// Creates Classes, variables & methods
        /// </summary>
        /// <param name="machineCode"></param>
        public static void Interporate(Script machineCode)
        {
            Debug.Log("[Compiler] Compile");

            toCompile = machineCode;

            string rawCode = machineCode.rawCode;
            if (!ConvertCode(rawCode, out convertedCode)) return;

            for (int i = 0; i < convertedCode.Count; i++)
            {
                string word = convertedCode[i];

                if (Keywords[KeywordType.Object].ContainsKey(word))
                {
                    Encapsulation encapsulation = (Encapsulation)Keywords[KeywordType.Object][word];

                    encapsulation.create.Invoke(i);
                }

                if(Keywords[KeywordType.Type].ContainsKey(word))
                {
                    Debug.Log(Keywords[KeywordType.Type][word].GetType());
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
