using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SharpCube.Highlighting
{
    public static class SyntaxHighlighting
    {
        public static ColorPallate ColorPallate = new ColorPallate();

        public static string RemoveHighlight(string code)
        {
            return Regex.Replace(code, "<.*?>", String.Empty);
        }
        public static string HighlightCode(string code)
        {
            string newCode = code;

            foreach (var type in Enum.GetValues(typeof(Keywords.Type)))
            {

                foreach (var keyword in Compiler.UniversalKeywords.keys[(Keywords.Type)type])
                {
                    newCode = Highlight(newCode,keyword.Key, keyword.Value.color);
                }
            }

            return newCode;
        }
        
        public static string Highlight(string code, string pattern, string color)
        {
            return Regex.Replace(code, $"\\b{Regex.Escape(pattern)}\\b", $"<color={color}>{pattern}</color>");
        }
    }

}