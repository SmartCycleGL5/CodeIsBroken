using SharpCube;
using SharpCube.Object;
using System;
using UnityEngine;

public interface IType
{
    public Type type { get; set; }
    public Type GetType()
    {
        return type;
    }

    public static void Create(int line, Properties properties)
    {
        Variable.Create(line, properties);
    }
}
