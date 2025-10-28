using SharpCube;
using SharpCube.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IType
{
    public Type type { get; set; }
    public Type GetType()
    {
        return type;
    }

    public static void Create(Encapsulation encapsulation, List<string> context, int line, Properties properties)
    {
        Variable.Create(encapsulation, context, line, properties);
    }
}
