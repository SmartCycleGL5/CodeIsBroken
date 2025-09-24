using System;
using UnityEngine;

namespace Coding.Language
{
    //[Serializable]
    public class Variable : Object<IVariableContainer>
    {
        //public IVariable(string name, IVariableContainer container, object value, Type type) : base (name, container, type)
        //{
        //    this.value = value;
        //}

        //protected void SetValue(object value)
        //{
        //    this.value = value;
        //}
        public Variable(string name, IVariableContainer container, Type type) : base(name, container, type)
        {
        }
    }
}
