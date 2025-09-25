using System;
using UnityEngine;

namespace Coding.Language
{
    [Serializable]
    public class Variable : Object<IVariableContainer>
    {
        public Variable(string name, IVariableContainer container, Type type) : base(name, container, type)
        {
            container.Add(this);

            Debug.Log("[Variable] New variable \"" + name + "\" of type: " + Type());
        }

        public virtual Type Type()
        {
            return Language.Type.Fail;
        }

        public static Variable NewVariable(Type type, string name, IVariableContainer container, object value = null)
        {
            switch(type)
            {
                case Language.Type.Int:
                    {
                        return new Int(name, container, (int)value);
                    }
                case Language.Type.Float:
                    {
                        return new Float(name, container, (float)value);
                    }
                case Language.Type.String:
                    {
                        return new String(name, container, (string)value);
                    }
                case Language.Type.Bool:
                    {
                        return new Bool(name, container, (bool)value);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
