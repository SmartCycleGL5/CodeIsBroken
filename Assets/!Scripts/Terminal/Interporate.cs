using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Coding
{
    using Language;
    using UnityEngine;

    [Serializable]
    public static class Interporate
    {
        public static void Classes(string script, BaseMachine machine)
        {
            //the script split into individual lines
            string[] scriptLines = ExtractLines(script).ToArray();

            for (int i = 0; i < scriptLines.Length; i++)
            {
                if (!scriptLines[i].Contains("class")) continue;

                string[] sections = scriptLines[i].Split(" ");
                List<string> classScript = scriptLines.ToList();
                Utility.FindEncapulasion(ref classScript, i, out int end, '{', '}');

                string name = sections[1];

                Class newClass = new Class(machine, name, classScript);
                Debug.Log("[Interperator] New Class for " + machine);
                machine.Classes.Add(name, newClass);

                i += end - i; //skips to the end of the class
            }
        }

        public static void MethodsAndVariables(string[] code, int line, out int end, Class @class)
        {
            end = line;

            List<string> sections = code[line].Split(" ").ToList();

            Utility.FindAndRetain(ref sections, '"', '"');
            Utility.FindAndRetain(ref sections, '(', ')');

            string name = sections[1];
            string type = sections[0];
            bool isMethod = name.Contains("(");

            if (!isMethod && !@class.variables.ContainsKey(name))
            {
                IVariable newVariable = @class.NewVariable(name, null, GetType(type));

                //Setting variables
                if (code[line].Contains("="))
                {
                    string value = sections[3];

                    newVariable.Set(value);
                }
            }
            else if (isMethod && !@class.methods.ContainsKey(name))
            {
                List<string> methodScript = @class.baseCode.ToList();

                Utility.FindEncapulasion(ref methodScript, line, out end, '{', '}');

                string arguments = name.Substring(name.IndexOf('('), 0);
                arguments.Replace("(", "");
                arguments.Replace(")", "");
                name = name.Substring(0, name.IndexOf('('));

                new UserMethod(
                    name: name,
                    arguments: null,
                    methodCode: methodScript.ToArray(),
                    @class: @class);
            }
        }

        public static Type GetType(string type)
        {
            switch(type)
            {
                case "void":
                    {
                        return Language.Type.Void;

                    }
                case "int":
                    {
                        return Language.Type.Int;
                    }
                case "float":
                    {
                        return Language.Type.Float;
                    }
                case "bool":
                    {
                        return Language.Type.Bool;
                    }
                case "string":
                    {
                        return Language.Type.String;
                    }
                default:
                    {
                        Debug.LogError("[Interporator] " + type + " is not a type");
                        return 0;
                    }
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

        public static object Type(string value)
        {
            object result = value;
            try
            {
                if (value.Contains('f'))
                {
                    result = float.Parse(value.Replace("f", ""));
                }
                else
                {
                    result = int.Parse(value);
                }
            }
            catch
            {

            }


            return new object();
        }

        public static object[] TranslateArguments(string args)
        {
            if (args == null || args == string.Empty) return null;

            Debug.Log("[ArgumentTranslation] " + args);

            string[] stringArgsList = Regex.Split(args, ",");

            List<object> argsList = new List<object>();

            foreach (var item in stringArgsList)
            {
                argsList.Add(Interporate.Type(item));
            }

            return argsList.ToArray();
        }
    }
}