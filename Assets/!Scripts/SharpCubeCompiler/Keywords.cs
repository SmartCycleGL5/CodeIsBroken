using System;
using System.Collections.Generic;
using SharpCube.Highlighting;
using UnityEngine;

namespace SharpCube
{
    public class Keywords
    {
        public enum Type
        {
            Initializer,
            Modifier,
            Reference,
            Valid,
        }

        public Dictionary<Type, Dictionary<string, Keyword>> keys = new(); 
        public static Keywords Default => new (new Dictionary<Type, Dictionary<string, Keyword>>()
        {
            {
                Type.Initializer, new()
            },

            {
                Type.Modifier, new()
            },

            {
                Type.Reference, new()
            },


            {
                Type.Valid, new()
            },
        });
        public static Keywords UniversalDefaultKeywords =>
            new(
                new()
                {
                    {
                        Type.Initializer, new()
                        {
                            { "class", new Initializer("class", Class.Create) },
                            //{ "struct", new Initializer(Color.blue, Class.Create) },

                            { "void", new Initializer("void", Method.Create) },
                            { "bool", new Initializer("bool", Variable.Create) },
                            { "int", new Initializer("int", Variable.Create) },
                            { "float", new Initializer("float", Variable.Create) },
                            { "string", new Initializer("string", Variable.Create) },
                        }
                    },

                    {
                        Type.Modifier, new()
                        {
                            { "private", new Modifier("private", Privilege.Private) },
                            { "public", new Modifier("public", Privilege.Public) },
                        }
                    },

                    {
                        Type.Reference, new()
                        {
                        }
                    },


                    {
                        Type.Valid, new()
                        {
                            { "{", new Keyword("{") },
                            { "}", new Keyword("}") },
                            { ";", new Keyword(";") },
                            { "(", new Keyword("(") },
                            { ")", new Keyword(")") },
                        }
                    },
                }
            );
        
        
        public Keywords(Dictionary<Type, Dictionary<string, Keyword>> keywords)
        {
            keys = keywords;
        }

        public bool Contains(string keyword)
        {
            foreach (var type in Enum.GetValues(typeof(Type)))
            {
                foreach (var key in keys[(Type)type])
                {
                    if(key.Key == keyword) return true;
                }
            }

            return false;
        }
        public void Add(Keywords toAdd)
        {
            foreach (var type in Enum.GetValues(typeof(Type)))
            {
                foreach (var keyword in toAdd.keys[(Type)type])
                {
                    keys[(Type)type].Add(keyword.Key, keyword.Value);   
                }
            }
        }
    }   
}
