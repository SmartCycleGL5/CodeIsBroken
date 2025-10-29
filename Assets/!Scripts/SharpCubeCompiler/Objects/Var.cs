using System;
using UnityEngine;
[Serializable]
public class Var : IType
{
    [field:SerializeField]public Type type { get; set; }
    public object value { get; set; }

    public Var(Type type, object value)
    {
        this.type = type;
        this.value = value;
    }
}
