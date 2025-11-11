using AYellowpaper.SerializedCollections;
using Coding.SharpCube;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Coding
{
    public static class SyntaxHighlighting
    {

        public static string ReturnHighlightedString(string text)
        {
            string[] splitString = text.Split(' ');



            for (int i = 0; i < splitString.Length; i++)
            {
                foreach (var keyword in Syntax.keywords)
                {
                    if (splitString[i] != keyword.Value.word) continue;

                    string hex = keyword.Value.highlightColor.ToHexString();
                    splitString[i] = "<color=" + hex + ">" + splitString[i] + "</color>";

                    Debug.Log("[Highlighting] found word: " + splitString[i]);
                }
            }

            string finalString = string.Empty;

            foreach (var item in splitString)
            {
                finalString += item;
            }

            return finalString;
        }
    }

}