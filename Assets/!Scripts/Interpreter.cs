using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utility;
using System.Text.RegularExpressions;

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
public static class Interpreter
{
    public static void InterperateInitialization(string script, ref MachineScript machine)
    {
        //the script split into individual lines
        string[] scriptLines = ExtractLines(script);

        foreach (var item in scriptLines)
        {
            Debug.Log(item);
        }

        for (int i = 0; i < scriptLines.Length; i++)
        {
            string[] sections = scriptLines[i].Split(" ");

            for (int j = 0; j < sections.Length; j++)
            {
                if (sections[j] == "class")
                {
                    List<string> classScript = scriptLines.ToList();

                    for (int k = i; k >= 0; k--)
                    {
                        classScript.RemoveAt(k);
                    }

                    machine.Classes.Add(sections[j + 1], new Class(classScript.ToArray()));
                }
            }
        }
    }

    /// <summary>
    /// initializes the clas
    /// </summary>
    /// <param name="ClassScript"></param>
    public static void InitializeClass(Class ClassScript)
    {
        foreach (var line in ClassScript.baseCode)
        {
            List<string> sections = line.Split(" ").ToList();

            FindAndRetainStrings(ref sections);

            if (ReturnType(sections[0], out Type type) && !ClassScript.variables.ContainsKey(sections[1]))
            {
                var value = new Dynamic(type);
                ClassScript.variables.Add(sections[1], value);
            }

            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i] == "=")
                {
                    Assignment(ClassScript.variables[sections[i - 1]], sections[i + 1]);
                    break;
                }
            }
        }
    }

    static string[] ExtractLines(string raw)
    {
        string modified = raw.Replace("\n", "");
        return Regex.Split(modified, "[;{}]");
    }

    static void Assignment(Dynamic variable, string toAssign)
    {
        variable.SetValue(toAssign);
    }
    static bool ReturnType(string line, out Type type)
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
}