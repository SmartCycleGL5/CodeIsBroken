using Coding.SharpCube.Lines;
using System;
using System.Collections.Generic;
using System.Reflection;
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

            { key.Void, new Keyword.Variable("void", Color.blue, VariableType.Void) },
            { key.Int, new Keyword.Variable("int", Color.blue, VariableType.Int) },
            { key.Float, new Keyword.Variable("float", Color.blue, VariableType.Float) },
            { key.String, new Keyword.Variable("string", Color.blue, VariableType.String) },
            { key.Bool, new Keyword.Variable("bool", Color.blue, VariableType.Bool) },

            { key.If, new Keyword.Encapsulation("if", Color.blue /*,Interporate.If*/) },
            { key.Else, new Keyword.Encapsulation("else", Color.blue) },

            { key.Return, new Keyword("return", Color.pink) },
            { key.Continue, new Keyword("continue", Color.pink) },
        };
    }

    public class Keyword
    {
        public string word;
        public Color highlightColor;

        public Keyword(string word, Color highlightColor)
        {
            this.word = word;
            this.highlightColor = highlightColor;
        }

        public class Variable : Keyword
        {
            public VariableType type;

            public Variable(string word, Color highlightColor, VariableType type = VariableType.Fail) : base(word, highlightColor)
            {
                this.type = type;
            }
        }
        public class Encapsulation : Keyword
        {
            Action<object[]> interporation;
            public Encapsulation(string word, Color highlightColor, Action<object[]> interporation = null) : base(word, highlightColor)
            {
                this.interporation = interporation;
            }

            public void Interporate(object[] parameters)
            {
                interporation.Invoke(parameters);
            }

        }
    }
}
