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
    }
    public static class Syntax
    {
        public static readonly Dictionary<key, string> keywords = new()
        {
            { key.Class, "class" },
            { key.Void, "void" },
            { key.Int, "int" },
            { key.Float, "float" },
            { key.String, "string" },
            { key.Bool, "bool" },
            { key.If, "if" },
        };
    }
}
