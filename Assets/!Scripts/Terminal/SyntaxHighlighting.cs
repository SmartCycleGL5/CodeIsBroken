using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SharpCube.Highlighting
{
    public class SyntaxHighlighting
    {
        public ColorPallate colorPallate { get; private set; } = new ColorPallate();

        Dictionary<string, ColorPallate.Type> keywords => new()
        {
            { "using", ColorPallate.Type.referenceColor },
            { "namespace", ColorPallate.Type.referenceColor },

            { "true", ColorPallate.Type.referenceColor },
            { "false", ColorPallate.Type.referenceColor },

            { "return", ColorPallate.Type.jumpColor },
            { "continue", ColorPallate.Type.jumpColor },
            { "break", ColorPallate.Type.jumpColor },
            { "goto", ColorPallate.Type.jumpColor },

            { "while", ColorPallate.Type.loopColor },
            { "for", ColorPallate.Type.loopColor },
            { "foreach", ColorPallate.Type.loopColor },

            { "void", ColorPallate.Type.initializerColor },
            { "int", ColorPallate.Type.initializerColor },
            { "float", ColorPallate.Type.initializerColor },
            { "string", ColorPallate.Type.initializerColor },
            { "bool", ColorPallate.Type.initializerColor },

            { "class", ColorPallate.Type.initializerColor },
            { "struct", ColorPallate.Type.initializerColor },
            { "enum", ColorPallate.Type.initializerColor },

            { "private", ColorPallate.Type.modifierColor },
            { "public", ColorPallate.Type.modifierColor },
            { "protected", ColorPallate.Type.modifierColor },
            { "internal", ColorPallate.Type.modifierColor },
            
            { "static", ColorPallate.Type.modifierColor },

            { "if", ColorPallate.Type.conditionalColor },
            { "else", ColorPallate.Type.conditionalColor },
            { "switch", ColorPallate.Type.conditionalColor },
        };

        public string RemoveHighlight(string code)
        {
            return Regex.Replace(code, @"<\/?color.*?>", String.Empty);
        }
        public string HighlightCode(string code)
        {
            string newCode = code;

            foreach (var keyword in keywords)
            {
                newCode = Highlight(newCode, keyword.Key, ColorUtility.ToHtmlStringRGB(colorPallate.Colors[keyword.Value]));
            }

            return newCode;
        }
        
        public string Highlight(string code, string pattern, string color)
        {
            return Regex.Replace(code, $"\\b{Regex.Escape(pattern)}\\b", $"<color=#{color}>{pattern}</color>");
        }

        public void SetPallate(ColorPallate pallate)
        {
           colorPallate = pallate;
        }
    }

}