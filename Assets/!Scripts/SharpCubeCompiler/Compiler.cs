using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SharpCube.Highlighting;
using UnityEngine;
using SharpCube.Type;

namespace SharpCube
{
    [Serializable]
    public struct Line : IEquatable<Line>
    {
        public Guid id {get; private set;}
        public string[] sections;
        public Line(string[] sections)
        {
            this.sections = sections;
            id = Guid.NewGuid();
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

        public bool Equals(Line other)
        {
            return Equals(GetLine(), other.GetLine());
        }
    }

    public static class Compiler
    {
        public static List<Line> convertedCode = new List<Line>();
        public static List<IContainer> containersToCompile = new();
        public static Keywords UniversalKeywords { get; private set; } = Keywords.UniversalDefaultKeywords;

        public static Script toCompile;

        /// <summary>
        /// Prepare Compiling
        /// </summary>
        /// <param name="script">The script to compile</param>
        public static void StartCompile(Script script)
        {
            Debug.Log("[Compiler] Starting Compile");
            
            toCompile = script;

            string rawCode = script.rawCode;
            if (!ConvertCode(rawCode, out convertedCode)) return;

            Compile(convertedCode);

            while (true)
            {
                List<IContainer> toCompile = containersToCompile;
                containersToCompile = new();
                
                Debug.Log("[Compiler] Done, starting containers: "  + toCompile.Count );

                for (int i = 0; i < toCompile.Count; i++)
                {
                    Debug.Log("[Compiler] starting " + toCompile[i]);
                    toCompile[i].StartCompile();

                    Debug.Log($"[Compiler] {toCompile[i]} done, {toCompile.Count - (i + 1)} left!");
                }

                if (containersToCompile.Count <= 0)
                {
                    Debug.Log("[Compiler] Finish");
                    break;   
                }
            }

            Debug.Log("[Compiler] Finished " + script.name);
            
            bool ConvertCode(string raw, out List<Line> convertedCode)
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
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static void Compile(List<Line> context, IContainer container = null)
        {
            Keywords currentKeywords = UniversalKeywords;

            Debug.Log($"[Compiler] {container} univarsalkeywords : " + currentKeywords.ToString());

            if (container != null)
                currentKeywords.Combine(container.allKeywords);

      
                
            Properties currentModifiers = null;

            for (int line = 0; line < context.Count; line++)
            {
                Debug.Log("[Compiler] Line: " + context[line].GetLine());

                for (int section = 0; section < context[line].sections.Length; section++)
                {
                    
                    string word = context[line].sections[section];
                    
                    if (!ValidKeyword(word))
                        PlayerConsole.LogError($"{word} does not exist in the current context");

                    //Debug.Log("[Compiler] Section: " +  word);

                    if (word == currentKeywords.keys[Keywords.Type.Valid][";"].name)
                        break;
                    
                    if (word == currentKeywords.keys[Keywords.Type.Valid]["}"].name)
                        PlayerConsole.LogError("Unexpected token \"}\"");


                    if (word == currentKeywords.keys[Keywords.Type.Valid]["{"].name)
                    {
                        line = Encapsulation.FindEndOfEndEncapsulation(line, context);
                        //Debug.Log($"[Compiler] Skipping to {context[line].GetLine()}");
                        break;
                    }
                    
                    if (currentKeywords.keys[Keywords.Type.Reference].ContainsKey(word))
                    {/*
                        Reference reference = (Reference)currentKeywords.keys[Keywords.Type.Reference][word];
                        PlayerConsole.Log($"Found reference {reference.reference.name} of type {reference.reference.Get()}");
                        continue;*/
                    }

                    if (currentKeywords.keys[Keywords.Type.Modifier].ContainsKey(word))
                    {
                        if (currentModifiers == null) currentModifiers = new();
                        if (currentModifiers.privilege != Privilege.None) PlayerConsole.LogError("Cannot have more than one protection modifier");

                        currentModifiers.privilege = ((Modifier)currentKeywords.keys[Keywords.Type.Modifier][word]).privilege;
                        continue;
                    }

                    if (currentKeywords.keys[Keywords.Type.Initializer].ContainsKey(word))
                    {
                        
                        Initializer initializer = (Initializer)currentKeywords.keys[Keywords.Type.Initializer][word];
                        
                        if(currentModifiers == null) currentModifiers = new(Privilege.Private);
                        
                        initializer.Intialize.Invoke(container, context[line], section, currentModifiers);
                        currentModifiers = null;

                        section++;
                    }
                    
                    if(currentModifiers != null) PlayerConsole.LogError($"Unexpected Token {currentModifiers.privilege}");
                }
            }
            
            bool ValidKeyword(string word)
            {
                foreach (var item in Enum.GetValues(typeof(Keywords.Type)))
                {
                    if (currentKeywords.keys[(Keywords.Type)item].ContainsKey(word))
                        return true;
                }
            
                return false;
            }
        }
        
        public static void Reset()
        {
            Debug.Log("[Compiler] Reset");
            toCompile = null;
            containersToCompile = new();
            UniversalKeywords = Keywords.UniversalDefaultKeywords;
        }
    }
}

