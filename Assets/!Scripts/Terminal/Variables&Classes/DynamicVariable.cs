using System;
using UnityEngine;

namespace Coding.Language
{
    [Serializable]
    public class Variable : Object<IVariable>
    {
        public Variable(string name, IVariable container, object value, Type type) : base (name, container, type)
        {
            this.value = value;
        }

        protected void SetValue(object value)
        {
            this.value = value;
        }
    }
}
