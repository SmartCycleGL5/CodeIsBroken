
using SharpCube.Object;

using SharpCube.Highlighting;
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
        Valid
    }

    public static partial class Compiler
    {
        
        public static string initializerColor = "#5397D0";
        public static string modifierColor = "#5397D0";
        public static string defaultColor = "#ffffff";

        public static readonly Dictionary<KeywordType, Dictionary<string, Keyword>> Keywords = new()
        {
            { KeywordType.Initializer, new()
            {
                { "class", new Initializer("class", initializerColor, Class.Create) },
                //{ "struct", new Initializer(Color.blue, Class.Create) },
                
                { "void", new Initializer("void", initializerColor, IType.Create)  },
                { "bool", new Initializer("bool", initializerColor, IType.Create)  },
                { "int", new Initializer("int", initializerColor, IType.Create)  },
                { "float", new Initializer("float", initializerColor, IType.Create) },
                { "string", new Initializer("string", initializerColor, IType.Create)  },
            } },

            { KeywordType.Modifier, new()
            {
                { "private", new Modifier("private", modifierColor, Privilege.Private) },
                { "public",  new Modifier("public", modifierColor, Privilege.Public) },
            } },
            
            { KeywordType.Valid, new()
            {
                { "{", new Keyword("{", defaultColor) },
                { "}",  new Keyword("}", defaultColor) },
                { ";",  new Keyword(";", defaultColor) },
            } },
        };

        public static Script toCompile;
        public static List<string> convertedCode;

        /// <summary>
        /// Prepare Compiling
        /// </summary>
        /// <param name="script">The script to compile</param>
        public static void StartCompile(Script script)
        {
            Debug.Log("[Compiler] Compile");

            Class.ClearClasses();

            toCompile = script;

            string rawCode = script.rawCode;
            if (!ConvertCode(rawCode, out convertedCode)) return;
            
            Compile(convertedCode);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Compile(List<string> toCompile)
        {
            Properties currentModifiers = new();
            
            for (int i = 0; i < convertedCode.Count; i++)
            {
                string word = toCompile[i];
                
                Debug.Log($"[Compiler] {word}");

                if(!ValidKeyword(word))
                {
                    PlayerConsole.LogError($"{word} does not exist in the current context");
                    return;
                }
                if (word == Keywords[KeywordType.Valid]["}"].key)
                {
                    PlayerConsole.LogError("Unexpected token \"}\"");
                    return;
                }

                if (word == Keywords[KeywordType.Valid]["{"].key)
                {
                    i = Encapsulation.FindEndOfEndEncapsulation(i) ;
                    continue;
                }

                if(Keywords[KeywordType.Modifier].ContainsKey(word))
                {
                    currentModifiers.privilege = ((Modifier)Keywords[KeywordType.Modifier][word]).privilege;
                    continue;
                }

                if (Keywords[KeywordType.Initializer].ContainsKey(word))
                {
                    Initializer encapsulation = (Initializer)Keywords[KeywordType.Initializer][word];

                    encapsulation.create.Invoke(i, currentModifiers);
                    currentModifiers = new();

                    i++;
                    continue;
                }
            }
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

