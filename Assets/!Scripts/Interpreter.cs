using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Terminal
{
    using Language;

    [Serializable]
    public static class Interpreter
    {
        public static void InterperateInitialization(string script, BaseMachine machine)
        {
            //the script split into individual lines
            string[] scriptLines = ExtractLines(script).ToArray();

            for (int i = 0; i < scriptLines.Length; i++)
            {
                if (scriptLines[i].Contains("class"))
                {
                    string[] sections = scriptLines[i].Split(" ");
                    string name = sections[1];

                    List<string> classScript = scriptLines.ToList();

                    FindEncapulasion(ref classScript, i);

                    Class newClass = new Class(machine, name, classScript);
                    machine.Classes.Add(name, newClass);
                }
            }
        }

        public static void FindEncapulasion(ref List<string> encapsulatedScript, int startPoint)
        {
            for (int k = startPoint + 1; k >= 0; k--)
            {
                encapsulatedScript[k] = "removed";
            }

            int encapsulations = 1;

            for (int k = 0; k < encapsulatedScript.Count; k++) //k start at start point?
            {

                if (encapsulations == 0)
                {
                    encapsulatedScript[k] = "removed";

                }
                else
                {
                    if (encapsulatedScript[k].Contains("{"))
                    {
                        encapsulations++;
                    }
                    else if (encapsulatedScript[k].Contains("}"))
                    {
                        encapsulations--;

                        if (encapsulations == 0)
                        {
                            encapsulatedScript[k] = "removed";
                        }
                    }
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

        public static void Assignment(Variable variable, string toAssign)
        {
            variable.SetValue(toAssign);
        }
        public static bool ReturnType(string line, out Type type)
        {
            type = Type.Void;
            switch (line)
            {
                case "void":
                    {
                        type = Type.Void;
                        return true;
                    }
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
}