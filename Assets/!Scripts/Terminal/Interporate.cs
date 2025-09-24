using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Coding
{
    using Language;
    using UnityEngine;
    using static Language.Syntax;

    [Serializable]
    public static class Interporate
    {
        public static void Classes(string script, BaseMachine machine)
        {
            //the script split into individual lines
            string[] scriptLines = ExtractLines(script).ToArray();

            for (int i = 0; i < scriptLines.Length; i++)
            {
                string[] sections = scriptLines[i].Split(" ");

                int index = Array.IndexOf(sections, keywords[key.Class].word);
                if (index < 0) continue;


                List<string> classScript = scriptLines.ToList();
                classScript = Utility.FindEncapulasion(
                    encapsulatedScript: classScript, 
                    startPoint: i, 
                    endPoint: out int end, 
                    startEncapsulation: '{', 
                    endEncapsulation: '}');

                string className = sections[index + 1];

                new Class(machine, className, classScript);

                i += end - i; //skips to the end of the class
            }
        }

        public static void Variables(Class @class)
        {

            string[] baseCode = @class.baseCode;

            for (int i = 0; i < baseCode.Length; i++)
            {
                if (baseCode[i].Contains('(')) continue;

                List<string> sections = SplitLineIntoSections(baseCode[i]);
                int index = -1;

                if (LineIsType(sections, key.Int, out index) > 0) 
                    DefineVariable(sections, index, Language.Type.Int);

                else if (LineIsType(sections, key.Float, out index) > 0)
                    DefineVariable(sections, index, Language.Type.Float);

                else if(LineIsType(sections, key.String, out index) > 0)
                    DefineVariable(sections, index, Language.Type.String);

                else if(LineIsType(sections, key.Bool, out index) > 0)
                    DefineVariable(sections, index, Language.Type.Bool);
            }

            int LineIsType(List<string> sections, key key, out int index)
            {
                index = Array.IndexOf(sections.ToArray(), keywords[key].word);
                return index;
            }
            void DefineVariable(List<string> sections, int index, Type type)
            {
                string name = sections[index + 1];
                @class.NewVariable(name, null, Language.Type.Int);
            }

        }
        public static void Methods(Class @class)
        {

            for (int i = 0; i < @class.baseCode.Length; i++)
            {

            }

        }

        public static List<string> SplitLineIntoSections(string line)
        {
            List<string> sections = line.Split(" ").ToList();
            Utility.FindAndRetain(ref sections, '"', '"');
            Utility.FindAndRetain(ref sections, '(', ')');

            return sections;
        }

        //public static void MethodsAndVariables(string[] code, int line, out int end, Class @class)
        //{
        //    end = line;

        //    List<string> sections = code[line].Split(" ").ToList();

        //    Utility.FindAndRetain(ref sections, '"', '"');
        //    Utility.FindAndRetain(ref sections, '(', ')');

        //    string name = sections[1];
        //    string type = sections[0];
        //    bool isMethod = name.Contains("(");

        //    if (!isMethod && !@class.variables.ContainsKey(name))
        //    {
        //        if (code[line].Contains("="))                //Setting variables
        //        {
        //            Variable newVariable = @class.NewVariable(name, sections[3], GetType(type));
        //        }
        //        else
        //        {
        //            Variable newVariable = @class.NewVariable(name, null, GetType(type));
        //        }
        //    }
        //    else if (isMethod && !@class.methods.ContainsKey(name))
        //    {
        //        List<string> methodScript = @class.baseCode.ToList();

        //        Utility.FindEncapulasion(ref methodScript, line, out end, '{', '}');

        //        string arguments = name.Substring(name.IndexOf('('), 0);
        //        arguments.Replace("(", "");
        //        arguments.Replace(")", "");
        //        name = name.Substring(0, name.IndexOf('('));

        //        new UserMethod(
        //            name: name,
        //            parameters: null,
        //            methodCode: methodScript.ToArray(),
        //            @class: @class);
        //    }
        //}


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