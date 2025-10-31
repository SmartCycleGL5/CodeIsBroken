using System;
using UnityEngine;
[Serializable]
public class Var : IType
{
    [field: SerializeField] public Type type { get; set; }
    [HideInInspector]public object value { get; set; }

    public Var(Type type, object value = null)
    {
        this.type = type;

        if (value == null)
        {
            switch (type)
            {
                case Type.String:
                    {
                        value = "";
                        break;
                    }
                case Type.Int:
                    {
                        value = 0;
                        break;
                    }
                case Type.Float:
                    {
                        value = 0f;
                        break;
                    }
                case Type.Bool:
                    {
                        value = false;
                        break;
                    }
            }
        }
        else
        {
            this.value = value;
        }
    }
}
