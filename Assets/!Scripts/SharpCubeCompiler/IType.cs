using SharpCube;
using SharpCube.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Float,
    Int,
    String,
    Bool
}

public interface IType
{

    public Type type {  get; set; }

    public static void Create(Encapsulation encapsulation, Line line, Properties properties)
    {
        string name = line.sections[line.sections.Length - 2];
        Type type = Parse(line.sections[line.sections.Length - 3]);
        new Variable(name, type, properties, encapsulation);
    }

    public static Type Parse(string value)
    {
        Enum.TryParse(value, true, out Type type);
        return type;
    }
}
