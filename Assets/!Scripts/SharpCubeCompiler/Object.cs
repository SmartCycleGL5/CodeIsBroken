using System;
using SharpCube.Type;
using UnityEngine;

namespace SharpCube
{
    public class Object
    {
        IType value;

        public Object(IType value = null)
        {
            Set(value);
        }

        public void Set(IType newValue)
        {
            if (newValue.GetType() == value.GetType())
                value = newValue;
        }
        public IType Get()
        {
            return value;
        }
    }

}