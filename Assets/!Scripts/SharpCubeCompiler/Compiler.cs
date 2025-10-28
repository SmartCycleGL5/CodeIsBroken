
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

    [Serializable]
    public struct Line
    {
        public string[] sections; 
        public Line(string[] sections)
        {
            this.sections = sections;
        }
    }

    public static class Compiler
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
            if (!ConvertCode(rawCode, out List<Line> convertedCode)) return;
            
            Compile(convertedCode);
        }

        public static List<Line> currentContext;
        /// <summary>
        /// 
        /// </summary>
        public static void Compile(List<Line> context, Encapsulation container = null)
        {
            currentContext = context;
            Properties currentModifiers = new();

            for (int line = 0; line < context.Count; line++)
            {
                for (int section = 0; section < context[line].sections.Length; section++)
                {
                    string word = context[line].sections[section];
                    
                    if (!ValidKeyword(word))
                    {
                        PlayerConsole.LogError($"{word} does not exist in the current context");
                    }
                    if (word == Keywords[KeywordType.Valid]["}"].key)
                    {
                        PlayerConsole.LogError("Unexpected token \"}\"");
                    }

                    if (word == Keywords[KeywordType.Valid]["{"].key)
                    {
                        line = Encapsulation.FindEndOfEndEncapsulation(line, context);
                        break;
                    }

                    if (Keywords[KeywordType.Modifier].ContainsKey(word))
                    {
                        currentModifiers.privilege = ((Modifier)Keywords[KeywordType.Modifier][word]).privilege;
                        continue;
                    }

                    if (Keywords[KeywordType.Initializer].ContainsKey(word))
                    {
                        Initializer encapsulation = (Initializer)Keywords[KeywordType.Initializer][word];

                        encapsulation.create.Invoke(container, context[line], currentModifiers);
                        currentModifiers = new();

                        line++;
                        continue;
                    }
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

            if (Class.initializedClasses.inMemory.ContainsKey(word))
            {
                return true;
            }
            
            foreach(var @class in Class.initializedClasses.inMemory)
            {
                if(@class.Value.Encapsulation.variables.Contains(word))
                {
                    return true;
                }   

            }
            return false;
        }

        static bool ConvertCode(string raw, out List<Line> convertedCode)
        {
            convertedCode = new();
            List<string> rawSplit = Regex.Split(raw, "( |\t|\n|;|}|{)").ToList();
            rawSplit.RemoveAll(x => x == "\t" || x == "\n" || x == " " || x == "");

            List<string> toAdd = new();
            foreach (var item in rawSplit)
            {
                if (item == ";")
                {
                    toAdd.Add(item);
                    convertedCode.Add(new Line(toAdd.ToArray()));
                    toAdd.Clear();
                    continue;
                }

                if (item == "{" || item == "}")
                {
                    convertedCode.Add(new Line(toAdd.ToArray()));
                    convertedCode.Add(new Line(new string[]{item}));
                    toAdd.Clear();
                    continue;
                }
                
                toAdd.Add(item);
            }

            return true;
        }
    }
}

