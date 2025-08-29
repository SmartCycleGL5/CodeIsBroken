using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    None,
    Float,
    Int,
    String,
    Bool,
    Method
}

[Serializable]
public class Interpreter
{
    public Dictionary<string, Dynamic> variables;

    public void Interperate(string[] lines)
    {
        foreach (var line in lines)
        {
            string[] sections = line.Split(" ");

            switch (sections[0])
            {
                case "float":
                    {
                        var value = new Dynamic(.1f);
                        variables.Add(sections[1], value);
                        break;
                    }
                case "int":
                    {
                        variables.Add(sections[1], new Dynamic(1));
                        break;
                    }
                case "string":
                    {
                        break;
                    }
                case "bool":
                    {
                        break;
                    }
            }
        }

        foreach (var item in variables)
        {
            Debug.Log(item.Value.value);
        }
    }

    Type ReturnType(string line)
    {
        switch (line)
        {
            case "float":
                {
                    return Type.Float;
                }
            case "int":
                {
                    return Type.Int;
                }
            case "string":
                {
                    return Type.String;
                }
            case "bool":
                {
                    return Type.Bool;
                }
        }

        return 0;
    }
}

public class Dynamic
{
    public dynamic value;

    public Dynamic(dynamic value)
    {
        this.value = value;
    }
}
