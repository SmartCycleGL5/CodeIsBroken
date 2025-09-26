using System;
using UnityEngine;

namespace Coding.SharpCube
{
    [Serializable]
    public class Variable : Object<IVariableContainer>
    {
        public Variable(string name, IVariableContainer container, Type type) : base(name, container, type)
        {
            container.Add(this);

            Debug.Log("[Variable] New variable \"" + name + "\" of type " + Type());
        }

        public virtual Type Type()
        {
            return SharpCube.Type.Fail;
        }


        public static Variable NewVariable(Type type, string name, IVariableContainer container, string value = null)
        {
            switch(type)
            {
                case SharpCube.Type.Int:
                    {
                        value = value != null ? value : string.Empty;
                        int intValue = int.Parse(value);

                        return new Int(name, container, intValue);
                    }
                case SharpCube.Type.Float:
                    {
                        value = value != null ? value : string.Empty;
                        float floatValue = float.Parse(value);

                        return new Float(name, container, floatValue);
                    }
                case SharpCube.Type.String:
                    {
                        value = value != null ? value : string.Empty;
                        return new String(name, container, value);
                    }
                case SharpCube.Type.Bool:
                    {
                        value = value != null ? value : string.Empty;
                        bool boolValue = bool.Parse(value);

                        return new Bool(name, container, boolValue);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
