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
        Initializer,
        Modifier,
    }
    public static partial class Compiler
    {
        public readonly static Dictionary<KeywordType, Dictionary<string, IKeyword>> Keywords = new()
        {
            { KeywordType.Initializer, new()
            {
                { "class", new Initializer(Color.blue, Class.Create) },
                //{ "struct", new Initializer(Color.blue, Class.Create) },

                { "void", new Initializer(Color.blue, IType.Create)  },
                { "bool", new Initializer(Color.blue, IType.Create)  },
                { "int", new Initializer(Color.blue, IType.Create)  },
                { "float", new Initializer(Color.blue, IType.Create) },
                { "string", new Initializer(Color.blue, IType.Create)  },
            } },

            { KeywordType.Modifier, new()
            {
                { "private", new Modifier(Color.aliceBlue, Privilege.Private) },
                { "public",  new Modifier(Color.aliceBlue, Privilege.Public) },
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

            Class.ClearClasses();

            toCompile = machineCode;

            string rawCode = machineCode.rawCode;
            if (!ConvertCode(rawCode, out convertedCode)) return;

            Properties currentModifiers = new();
            
            for (int i = 0; i < convertedCode.Count; i++)
            {
                string word = convertedCode[i];

                if(!ValidKeyword(word))
                {
                    PlayerConsole.LogError($"The name {word} does not exist in the current context");
                    continue;
                }

                if(Keywords[KeywordType.Modifier].ContainsKey(word))
                {
                    currentModifiers.privilege = ((Modifier)Keywords[KeywordType.Modifier][word]).privilege;
                }

                if (Keywords[KeywordType.Initializer].ContainsKey(word))
                {
                    Initializer encapsulation = (Initializer)Keywords[KeywordType.Initializer][word];

                    encapsulation.create.Invoke(i, currentModifiers);
                    currentModifiers = new();
                    continue;
                }

                //if (Keywords[KeywordType.Type].ContainsKey(word))
                //{
                //    Debug.Log("[Compiler] " + $"Found new variable {convertedCode[i + 1]}");
                //    //Debug.Log("[Compiler] " + Keywords[KeywordType.Type][word].);
                //    continue;
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Compile()
        {

        }

        static bool ValidKeyword(string word)
        {
            foreach (var item in Enum.GetValues(typeof(KeywordType)))
            {
                if (Keywords[(KeywordType)item].ContainsKey(word))
                    return true;
            }

            if(Class.initializedClasses.@public.ContainsKey(word))
            {
                return true;
            }
            return false;
        }

        static bool ConvertCode(string raw, out List<string> convertedCode)
        {
            convertedCode = Regex.Split(raw, "( |\t|\n|;|}|{)").ToList();
            convertedCode.RemoveAll(x => x == "\t" || x == "\n" || x == " " || x == "");

            return true;
        }
    }
}
