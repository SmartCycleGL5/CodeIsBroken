using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.CoreUtils;

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
    [SerializedDictionary("Name", "Value")]
    public SerializedDictionary<string, Dynamic> variables;

    public void Interperate(string[] lines)
    {
        foreach (var line in lines)
        {
            // splits the command into sections allowing us to look at each word individually
            List<string> sections = line.Split(" ").ToList();

            FindAndRetainStrings(ref sections);

            if (ReturnType(sections[0], out Type type) && !variables.ContainsKey(sections[1]))
            {
                var value = new Dynamic(type);
                variables.Add(sections[1], value);
            }

            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i] == "=")
                {
                    Assignment(variables[sections[i - 1]], sections[i + 1]);
                    break;
                }
            }
        }
    }

    void Assignment(Dynamic variable, string toAssign)
    {
        variable.SetValue(toAssign);
    }
    bool ReturnType(string line, out Type type)
    {
        type = Type.None;
        switch (line)
        {
            case "float":
                {
                    type = Type.Float;
                    return true;
                }
            case "int":
                {
                    type = Type.Int;
                    return true;
                }
            case "string":
                {
                    type = Type.String;
                    return true;
                }
            case "bool":
                {
                    type = Type.Bool;
                    return true;
                }
        }

        return false;
    }

    /// <summary>
    /// combines "" into a single section
    /// </summary>
    /// <param name="sections"></param>
    void FindAndRetainStrings(ref List<string> sections)
    {
        int? firstSection = null;
        int? secondSection = null;
        for (int i = 0; i < sections.Count; i++)
        {
            if (sections[i].Contains('"'))
            {
                if (firstSection == null)
                    firstSection = i;
                else
                {
                    secondSection = i;

                    for (int j = (int)firstSection + 1; j < secondSection + 1; j++)
                    {
                        sections[(int)firstSection] += " " + sections[j];

                        sections[j] = null;
                    }

                    firstSection = null;
                    secondSection = null;
                }
            }
        }

        sections.RemoveAll(x => x == null);
    }


    public void ClearMemory()
    {
        variables.Clear();
    }
}
[Serializable]
public class Dynamic
{
    public Type type;

    public float Float = 0;
    public int Int = 0;
    public string String = "";
    public bool Bool = false;

    public Dynamic(Type type)
    {
        this.type = type;
    }
    public void SetValue(string variable)
    {
        switch (type)
        {
            case Type.String:
                {
                    String = variable;
                    return;
                }
            case Type.Bool:
                {
                    if (variable == "true")
                    {
                        Bool = true;
                    }
                    else if (variable == "false")
                    {
                        Bool = false;
                    }
                    return;
                }
            case Type.Float:
                {
                    Float = float.Parse(variable);
                    return;
                }
            case Type.Int:
                {
                    Int = int.Parse(variable);
                    return;
                }
        }
    }
}