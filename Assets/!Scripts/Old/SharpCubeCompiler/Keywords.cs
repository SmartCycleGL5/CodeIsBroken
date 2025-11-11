using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using SharpCube.Highlighting;
using UnityEngine;
using UnityEngine.Rendering;

namespace SharpCube
{
    [Serializable]
    public class Keywords
    {
        public enum Type
        {
            Initializer,
            Modifier,
            Reference,
            Valid,
        }

        [field: SerializeField] public SerializedDictionary<Type, SerializedDictionary<string, Keyword>> keys { get;
            private set;
        } = new(); 
        public static Keywords Default => new (new SerializedDictionary<Type, SerializedDictionary<string, Keyword>>()
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
        
        
        public Keywords(SerializedDictionary<Type, SerializedDictionary<string, Keyword>> keywords)
        {
            keys = keywords;
        }

        public void Add(Type type, Keyword keyword)
        {
            Debug.Log($"[Keywords] Adding keyword: {keyword.name}");
            keys[type].Add(keyword.name, keyword);
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
        public Keywords Combine(Keywords toAdd)
        {
            foreach (var type in Enum.GetValues(typeof(Type)))
            {
                foreach (var keyword in toAdd.keys[(Type)type])
                {
                    Add((Type)type, keyword.Value);   
                }
            }

            return this;
        }

        public static Keywords Combine(Keywords k1, Keywords k2)
        {
            return k1.Combine(k2);
        }

        public override string ToString()
        {
            string String = "";

            foreach (var item in keys)
            {
                foreach (var keyword in item.Value)
                {
                    String += "\n" + keyword;
                }
            }

            return String;
        }
    }   
}
