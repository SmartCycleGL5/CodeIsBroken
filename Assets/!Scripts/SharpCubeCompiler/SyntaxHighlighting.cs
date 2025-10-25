using SharpCube.Object;
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
                    string color = keyword.Value.color;
                    
                    newCode = Regex.Replace(newCode, keyword.Key, $"<color={color}>{keyword.Key}</color>");
                }
            }

            Debug.Log(newCode);

            return newCode;
        }
    }

}