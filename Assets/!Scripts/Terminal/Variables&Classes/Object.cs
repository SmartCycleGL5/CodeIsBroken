using System;
using UnityEngine;

namespace Coding.Language
{
    public enum Type
    {
        Fail,
        Void,
        Float,
        Int,
        String,
        Bool
    }

    [Serializable]
    public abstract class Object<T>
    {
        public string name { get; private set; }
        [field: SerializeField] public Type type { get; private set; }
        public T container { get; private set; }

        public object value { get; protected set; }

        public Object(string name, T container, Type type)
        {
            this.name = name;
            this.container = container;
            this.type = type;
        }
    }

}