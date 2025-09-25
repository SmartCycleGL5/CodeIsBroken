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
        public ObjectInfo<T> info;

        public Object(string name, T container, Type type)
        {
            info.name = name;
            info.container = container;
            info.type = type;
        }
        public Object(ObjectInfo<T> info)
        {
            this.info = info;
        }

    }
    [Serializable]
    public struct ObjectInfo<T>
    {
        [HideInInspector] public string name;
        public Type type;
        [HideInInspector] public T container;
        [HideInInspector] public object value;

        public ObjectInfo(string name, Type type, T container, object value)
        {
            this.name = name;
            this.type = type;
            this.container = container;
            this.value = value;
        }
    }

}