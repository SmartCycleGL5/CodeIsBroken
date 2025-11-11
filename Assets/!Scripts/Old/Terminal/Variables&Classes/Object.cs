using System;
using UnityEngine;

namespace Coding.SharpCube
{
    public enum VariableType
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

        public Object(string name, T container, VariableType type)
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
        public VariableType type;
        [HideInInspector] public T container;
        [HideInInspector] public object value;

        public ObjectInfo(string name, VariableType type, T container, object value)
        {
            this.name = name;
            this.type = type;
            this.container = container;
            this.value = value;
        }
    }

}