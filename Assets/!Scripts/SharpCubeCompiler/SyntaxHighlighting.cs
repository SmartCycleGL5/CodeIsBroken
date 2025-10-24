using SharpCube.Object;
using System;
using UnityEngine;

namespace SharpCube.Highlighting
{
    public static class SyntaxHighlighting
    {
        public static void RemoveHighlight(ref string code)
        {

        }
        public static string HighlightCode(string code)
        {
            string[] splitCode = code.Split(" ");

            for (int i = 0; i < splitCode.Length; i++)
            {
                foreach (var key in Enum.GetValues(typeof(KeywordType)))
                {
                    if (!Compiler.Keywords[(KeywordType)key].ContainsKey(splitCode[i])) break;

                    string hexColor = ColorUtility.ToHtmlStringRGB(Compiler.Keywords[(KeywordType)key][splitCode[i]].color);

                    splitCode[i] = $"<color=#{hexColor}>{splitCode[i]}</color>";
                    continue;
                }

                if (Class.initializedClasses.@public.ContainsKey(splitCode[i]))
                {
                    continue;
                }
            }
            string newString = "";

            foreach (var item in splitCode)
            {
                newString += item + " ";
            }
            return newString;
        }
    }

}