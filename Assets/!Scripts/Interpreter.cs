using NaughtyAttributes.Test;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.CoreUtils;
using static Utility;

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
        string[] scriptLines = ExtractLines(script).ToArray();

        //make into method that finds encapulasions rather than just classes
        for (int i = 0; i < scriptLines.Length; i++)
        {
            if (scriptLines[i].Contains("class"))
            {
                string[] sections = scriptLines[i].Split(" ");
                string name = sections[1];

                List<string> classScript = scriptLines.ToList();

                FindEncapulasion(ref classScript, i);

                Class newClass = new Class(name, classScript.ToArray());
                machine.Classes.Add(name, newClass);

                InitializeClass(ref newClass);
            }
        }
    }

    /// <summary>
    /// initializes the class
    /// </summary>
    /// <param name="Class"></param>
    public static void InitializeClass(ref Class Class)
    {
        foreach (var line in Class.baseCode)
        {
            List<string> sections = line.Split(" ").ToList();

            FindAndRetainStrings(ref sections);

            if (ReturnType(sections[0], out Type type) && !Class.variables.ContainsKey(sections[1]))
            {
                var value = new Dynamic(type);
                Class.variables.Add(sections[1], value);
            }

            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i] == "=")
                {
                    Assignment(Class.variables[sections[i - 1]], sections[i + 1]);
                    break;
                }
            }
        }
    }

    static void FindEncapulasion(ref List<string> encapsulatedScript, int lineNum)
    {
        for (int k = lineNum + 1; k >= 0; k--)
        {
            encapsulatedScript[k] = "removed";
        }
        bool foundEnd = false;
        for (int k = 0; k < encapsulatedScript.Count; k++)
        {
            Debug.Log(encapsulatedScript[k]);
            if (foundEnd)
            {
                encapsulatedScript[k] = "removed";
            }
            else if (encapsulatedScript[k].Contains("}"))
            {
                encapsulatedScript[k] = "removed";
                foundEnd = true;
            }
        }

        encapsulatedScript.RemoveAll(item => item == "removed");
        encapsulatedScript.RemoveAll(item => item == "");
    }

    static List<string> ExtractLines(string raw)
    {
        //removes enter
        string modified = raw.Replace("\n", "");
        //removes tab
        modified = modified.Replace("\t", "");
        //splits it into a string array while keeping ; { and }
        List<string> list = Regex.Split(modified, "(;|{|})").ToList();
        //removes ;
        list.RemoveAll(item => item == ";");

        return list;
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