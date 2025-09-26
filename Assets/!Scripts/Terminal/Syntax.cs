using System.Collections.Generic;
using UnityEngine;

namespace Coding.Language
{
    public enum key
    {
        fail,

        Class,

        Void,
        Int,
        Float,
        String,
        Bool,

        If,
        Else,

        Return,
        Continue,
    }
    public static class Syntax
    {
        public static readonly Dictionary<key, Keyword> keywords = new()
        {
            { key.Class, new Keyword("class", Color.blue) },

            { key.Void, new Keyword("void", Color.blue) },
            { key.Int, new Keyword("int", Color.blue) },
            { key.Float, new Keyword("float", Color.blue) },
            { key.String, new Keyword("string", Color.blue) },
            { key.Bool, new Keyword("bool", Color.blue) },

            { key.If, new Keyword("if", Color.blue) },
            { key.Else, new Keyword("else", Color.blue) },

            { key.Return, new Keyword("return", Color.blue) },
            { key.Continue, new Keyword("continue", Color.blue) },
        };

        public struct Keyword
        {
            public string word;
            public Color highlightColor;

            public Keyword(string word, Color highlightColor)
            {
                this.word = word;
                this.highlightColor = highlightColor;
            }
        }
    }
}
