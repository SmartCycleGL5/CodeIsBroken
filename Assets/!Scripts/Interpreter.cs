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
        List<string> scriptLines = ExtractLines(script);

        foreach (var item in scriptLines)
        {
            Debug.Log(item);
        }


        //make into method that finds encapulasions rather than just classes
        for (int i = 0; i < scriptLines.Count; i++)
        {
            string[] sections = scriptLines[i].Split(" ");

            for (int j = 0; j < sections.Length; j++)
            {
                if (sections[j] == "class")
                {
                    List<string> classScript = scriptLines;

                    FindEncapulasion(ref classScript, i, j);

                    Class newClass = new Class(classScript.ToArray());
                    machine.Classes.Add(sections[j + 1], newClass);

                    InitializeClass(ref newClass);

                    break;
                }
            }
        }
    }

    /// <summary>
    /// initializes the clas
    /// </summary>
    /// <param name="ClassScript"></param>
    public static void InitializeClass(ref Class ClassScript)
    {
        foreach (var line in ClassScript.baseCode)
        {
            Debug.Log($"{line}");
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

    static void FindEncapulasion(ref List<string> encapsulatedScript, int lineNum, int sectionNum)
    {
        for (int k = lineNum + 1; k >= 0; k--)
        {
            encapsulatedScript[k] = "removed";
        }
        bool foundEnd = false;
        for (int k = sectionNum; k < encapsulatedScript.Count; k++)
        {
            if (foundEnd)
            {
                encapsulatedScript[k] = "removed";
            }
            else if (encapsulatedScript[k] == "}")
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
        string modified = raw.Replace("\n", "");
        modified = modified.Replace("\t", "");
        List<string> list = Regex.Split(modified, "(;|{|})").ToList();
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