using SharpCube;
using SharpCube.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IType
{
    public static void Create(Encapsulation encapsulation, Line line, Properties properties)
    {
        new Variable(line.sections[line.sections.Length - 2], properties, encapsulation);
    }
}
