using System;
using UnityEngine;

namespace Coding.Language
{
    public enum Type
    {
        Void,
        Float,
        Int,
        String,
        Bool
    }
    [Serializable]
    public class Variable : Object
    {

        //public Type type;

        //public float Float = 0;
        //public int Int = 0;
        //public string String = "";
        //public bool Bool = false;

        public Variable(string name, object value) : base (name)
        {
            this.value = value;
        }

        public void SetValue(object value)
        {
            Debug.Log("[Variable] set type: " + value.GetType());
        }

        //public void SetValue(string variable)
        //{
        //    switch (type)
        //    {
        //        case Type.String:
        //            {
        //                String = variable;
        //                return;
        //            }
        //        case Type.Bool:
        //            {
        //                if (variable == "true")
        //                {
        //                    Bool = true;
        //                }
        //                else if (variable == "false")
        //                {
        //                    Bool = false;
        //                }
        //                return;
        //            }
        //        case Type.Float:
        //            {
        //                Float = float.Parse(variable);
        //                return;
        //            }
        //        case Type.Int:
        //            {
        //                Int = int.Parse(variable);
        //                return;
        //            }
        //    }
        //}
    }
}
