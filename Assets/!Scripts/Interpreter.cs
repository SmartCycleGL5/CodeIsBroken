using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Coding
{
    using Language;
    using Unity.VisualScripting;
    using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

    [Serializable]
    public static class Interpreter
    {
        public static void InterperateScript(string script, BaseMachine machine)
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

                    Utility.FindEncapulasion(ref classScript, i, '{', '}');

                    Class newClass = new Class(machine, name, classScript);
                    machine.Classes.Add(name, newClass);
                }
            }
        }

        public static void DefineMethodsAndVariables(string[] code, int line, Class @class)
        {
            List<string> sections = code[line].Split(" ").ToList();

            Utility.FindAndRetain(ref sections);

            //Find variables & methods
            if (!ReturnType(sections[0], out Type type)) return;

            string name = sections[1];
            bool isMethod = name.Contains("(");

            if (!isMethod && !@class.variables.ContainsKey(name))
            {
                Variable newVariable = @class.NewVariable(name, type);

                //Setting variables
                if (code[line].Contains("="))
                {
                    string value = sections[3];

                    newVariable.SetValue(value);
                }
            }
            else if (isMethod && !@class.methods.ContainsKey(name))
            {
                List<string> methodScript = @class.baseCode.ToList();

                Utility.FindEncapulasion(ref methodScript, line, '{', '}');

                @class.NewMethod(name, methodScript.ToArray(), type);
            }
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