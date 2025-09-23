using System;
using UnityEngine;

namespace Coding.Language
{
    //[Serializable]
    public interface IVariable<T> : ISettable<T>, IGettable<T>
    {
        //public IVariable(string name, IVariableContainer container, object value, Type type) : base (name, container, type)
        //{
        //    this.value = value;
        //}

        //protected void SetValue(object value)
        //{
        //    this.value = value;
        //}
    }
}
