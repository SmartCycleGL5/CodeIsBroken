using System;
using UnityEngine;

namespace SharpCube
{
    public class Object
    {
        IReference value;

        public Object(IReference value = null)
        {
            this.value = value;
        }

        public void Set(IReference newValue)
        {
            if (newValue.GetType() == value.GetType())
                value = newValue;
        }
        public IReference Get()
        {
            return value;
        }
    }

}