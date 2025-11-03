using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using SharpCube.Type;

namespace SharpCube
{
    public enum KeywordType
    {
        Initializer,
        Modifier,
        Reference,
        Valid,
    }

    [Serializable]
    public struct Line
    {
        public string[] sections;
        public Line(string[] sections)
        {
            this.sections = sections;
        }

        public string GetLine()
        {
            string message = "";
            foreach (var section in sections)
            {
                message += " " + section.ToString();
            }

            return message;
        }
    }

    public static class Compiler
    {
        public static readonly string initializerColor = "#5397D0";
        public static readonly string modifierColor = "#5397D0";
        public static readonly string variableColor = "#33ccff";
        public static readonly string classColor = "#77ff77";
        public static readonly string methodColor = "#ffff33";
        public static readonly string defaultColor = "#ffffff";
        
        public static Dictionary<KeywordType, Dictionary<string, Keyword>> DefaultKeywords => new()
        {
            { KeywordType.Initializer, new()
            {
                { "class", new Initializer("class", initializerColor, Class.Create) },
                //{ "struct", new Initializer(Color.blue, Class.Create) },
                
                { "void", new Initializer("void", initializerColor, IType.CreateMethod)  },
                { "bool", new Initializer("bool", initializerColor, Variable.Create)  },
                { "int", new Initializer("int", initializerColor, Variable.Create)  },
                { "float", new Initializer("float", initializerColor, Variable.Create) },
                { "string", new Initializer("string", initializerColor, Variable.Create)  },
            } },

            { KeywordType.Modifier, new()
            {
                { "private", new Modifier("private", modifierColor, Privilege.Private) },
                { "public",  new Modifier("public", modifierColor, Privilege.Public) },
            } },
            
            { KeywordType.Reference, new()
            {
            } },


            { KeywordType.Valid, new()
            {
                { "{", new Keyword("{", defaultColor) },
                { "}",  new Keyword("}", defaultColor) },
                { ";",  new Keyword(";", defaultColor) },
                { "(",  new Keyword("(", defaultColor) },
                { ")",  new Keyword(")", defaultColor) },
            } },
        };

        public static Dictionary<KeywordType, Dictionary<string, Keyword>> Keywords = DefaultKeywords;

        public static Script toCompile;

        /// <summary>
        /// Prepare Compiling
        /// </summary>
        /// <param name="script">The script to compile</param>
        public static void StartCompile(Script script)
        {
            Debug.Log("[Compiler] Starting Compile");

            Class.ClearClasses();

            toCompile = script;

            string rawCode = script.rawCode;
            if (!ConvertCode(rawCode, out List<Line> convertedCode)) return;

            Compile(convertedCode);
            
            Debug.Log("[Compiler] Done, starting containers");
            
            foreach (var item in containersToCompile)
            {
                Debug.Log("[Compiler] starting " + item);
                item.StartCompile();
            }
        }

        public static List<Line> currentContext;
        public static List<IContainer> containersToCompile = new();
        /// <summary>
        /// 
        /// </summary>
        public static void Compile(List<Line> context, IContainer container = null)
        {
            currentContext = context;
            Properties currentModifiers = new();

            for (int line = 0; line < context.Count; line++)
            {
                Debug.Log("[Compiler] " + context[line].GetLine());

                for (int section = 0; section < context[line].sections.Length; section++)
                {
                    
                    string word = context[line].sections[section];

                    if (!ValidKeyword(word))
                        PlayerConsole.LogError($"{word} does not exist in the current context");

                    if (word == Keywords[KeywordType.Valid][";"].key)
                        break;
                    
                    if (word == Keywords[KeywordType.Valid]["}"].key)
                        PlayerConsole.LogError("Unexpected token \"}\"");


                    if (word == Keywords[KeywordType.Valid]["{"].key)
                    {
                        line = Encapsulation.FindEndOfEndEncapsulation(line, context);
                        Debug.Log($"[Compiler] Skipping to {context[line].GetLine()}");
                        break;
                    }
                    
                    if (Keywords[KeywordType.Reference].ContainsKey(word))
                    {
                        Debug.Log($"[Compiler] Found Reference to {word}");
                        continue;
                    }

                    if (Keywords[KeywordType.Modifier].ContainsKey(word))
                    {
                        currentModifiers.privilege = ((Modifier)Keywords[KeywordType.Modifier][word]).privilege;
                        continue;
                    }

                    if (Keywords[KeywordType.Initializer].ContainsKey(word))
                    {
                        
                        Initializer encapsulation = (Initializer)Keywords[KeywordType.Initializer][word];

                        encapsulation.create.Invoke(container, context[line], section, currentModifiers);
                        currentModifiers = new();

                        section++;
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
            
            return false;
        }

        static bool ConvertCode(string raw, out List<Line> convertedCode)
        {
            convertedCode = new();

            List<string> rawSplit = Regex.Split(raw, @"( |\t|\n|;|}|{|\(|\))").ToList();
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
                    //if(toAdd.Count > 0)
                    convertedCode.Add(new Line(toAdd.ToArray()));

                    convertedCode.Add(new Line(new string[] { item }));
                    toAdd.Clear();
                    continue;
                }

                toAdd.Add(item);
            }

            return true;
        }
        
        public static void Reset()
        {
            Debug.Log("[Compiler] Reset");
            toCompile = null;
            Keywords = DefaultKeywords;
        }
    }
}

