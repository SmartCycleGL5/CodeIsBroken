using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SharpCube.Highlighting
{
    public static class SyntaxHighlighting
    {


        public static string RemoveHighlight(string code)
        {
            return Regex.Replace(code, "<.*?>", String.Empty);
        }
        public static string HighlightCode(string code)
        {
            string newCode = code;

            foreach (var type in Enum.GetValues(typeof(KeywordType)))
            {

                foreach (var keyword in Compiler.Keywords[(KeywordType)type])
                {
                    newCode = Highlight(keyword.Key, keyword.Value.color);
                }
            }

            return newCode;

            string Highlight(string pattern, string color)
            {
                return Regex.Replace(newCode, $"\\b{Regex.Escape(pattern)}\\b", $"<color={color}>{pattern}</color>");
            }
        }
    }

}