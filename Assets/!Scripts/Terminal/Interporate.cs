using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Coding
{
    using Coding.Language.Actions;
    using Coding.Language.Lines;
    using Language;
    using UnityEngine;
    using static Language.Syntax;
    using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

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

            for (int line = 0; line < baseCode.Length; line++)
            {
                if (baseCode[line].Contains('(')) continue;

                List<string> sections = SplitLineIntoSections(baseCode[line]);
                int index = -1;

                for (int key = (int)Language.key.Int; key < (int)Language.key.Bool + 1; key++)
                {
                    if (!LineIsType((key)key, sections, out index)) continue;

                    string value = null;

                    int setPos = sections.IndexOf("=");
                    if (setPos >= 0)
                        value = sections[setPos + 1];


                    Variable.NewVariable(
                        type: keywords[(key)key].type, 
                        name: sections[index + 1], 
                        container: @class,
                        value: value);
                    break;
                }
            }

        }
        public static void Methods(Class @class)
        {
            string[] baseCode = @class.baseCode;

            for (int line = 0; line < baseCode.Length; line++)
            {
                if (!baseCode[line].Contains('(')) continue;

                List<string> sections = SplitLineIntoSections(baseCode[line]);
                int index = -1;

                for (int key = (int)Language.key.Void; key < (int)Language.key.Bool + 1; key++)
                {
                    if (!LineIsType((key)key, sections, out index)) continue;

                    List<string> methodScript = @class.baseCode.ToList();
                    methodScript = Utility.FindEncapulasion(methodScript, line, out int end, '{', '}');

                    new UserMethod(
                    name: sections[index + 1],
                    parameters: null,
                    methodCode: methodScript.ToArray(),
                    container: @class,
                    returnType: keywords[(key)key].type
                    );

                    line += end - line;
                    break;
                }
            }
        }
        public static IRunnable Line(string line, UserMethod method)
        {
            List<string> sections = SplitLineIntoSections(line);

            Line newLine = new Line();

            for (int i = 0; i < sections.Count; i++)
            {
                string section = sections[i];

                if(section.Contains("("))
                {
                    List<char> seperators = new() { '(', ')' };
                    string[] parameters = section.Split(seperators.ToArray());

                    string name = parameters[0];

                    newLine.Add(new MethodCall(name, method, parameters));
                }
                    
            }

            return null;
        }

        static bool LineIsType(key key, List<string> sections, out int index)
        {
            index = Array.IndexOf(sections.ToArray(), keywords[key].word);
            return index >= 0;
        }

        public static List<string> SplitLineIntoSections(string line)
        {
            List<string> sections = line.Split(" ").ToList();
            Utility.FindAndRetain(ref sections, '"', '"');
            Utility.FindAndRetain(ref sections, '(', ')');

            return sections;
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