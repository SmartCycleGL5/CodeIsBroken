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

            Debug.Log("[Variable] New variable \"" + name + "\" of type " + Type());
        }

        public virtual Type Type()
        {
            return Language.Type.Fail;
        }


        public static Variable NewVariable(Type type, string name, IVariableContainer container, string value = null)
        {
            switch(type)
            {
                case Language.Type.Int:
                    {
                        value = value != null ? value : string.Empty;
                        int intValue = int.Parse(value);

                        return new Int(name, container, intValue);
                    }
                case Language.Type.Float:
                    {
                        value = value != null ? value : string.Empty;
                        float floatValue = float.Parse(value);

                        return new Float(name, container, floatValue);
                    }
                case Language.Type.String:
                    {
                        value = value != null ? value : string.Empty;
                        return new String(name, container, value);
                    }
                case Language.Type.Bool:
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
