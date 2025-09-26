using Coding.SharpCube.Lines;
using System.Collections.Generic;
using UnityEngine;

namespace Coding.SharpCube
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

            { key.Void, new Keyword("void", Color.blue, Type.Void) },
            { key.Int, new Keyword("int", Color.blue, Type.Int) },
            { key.Float, new Keyword("float", Color.blue, Type.Float) },
            { key.String, new Keyword("string", Color.blue, Type.String) },
            { key.Bool, new Keyword("bool", Color.blue, Type.Bool) },

            { key.If, new Keyword("if", Color.blue) },
            { key.Else, new Keyword("else", Color.blue) },

            { key.Return, new Keyword("return", Color.pink) },
            { key.Continue, new Keyword("continue", Color.pink) },
        };

        public struct Keyword
        {
            public string word;
            public Color highlightColor;
            public Type type;

            public Keyword(string word, Color highlightColor, Type type = Type.Fail)
            {
                this.word = word;
                this.highlightColor = highlightColor;
                this.type = type;

            }
        }
    }
}
