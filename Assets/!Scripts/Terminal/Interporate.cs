using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Coding
{
    using Coding.SharpCube.Actions;
    using Coding.SharpCube.Encapsulations;
    using Coding.SharpCube.Lines;
    using SharpCube;
    using UnityEngine;
    using static SharpCube.Syntax;
    using static Utility;


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

                for (int key = (int)SharpCube.key.Int; key < (int)SharpCube.key.Bool + 1; key++)
                {
                    if (!LineIsType((key)key, sections, out index)) continue;

                    string value = null;

                    int setPos = sections.IndexOf("=");
                    if (setPos >= 0)
                        value = sections[setPos + 1];


                    Variable.NewVariable(
                        type: ((Keyword.Variable)keywords[(key)key]).type, 
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

                for (int key = (int)SharpCube.key.Void; key < (int)SharpCube.key.Bool + 1; key++)
                {
                    if (!LineIsType((key)key, sections, out index)) continue;

                    List<string> methodScript = @class.baseCode.ToList();
                    methodScript = Utility.FindEncapulasion(methodScript, line, out int end, '{', '}');


                    string name = sections[index + 1].Substring(0, sections[index + 1].IndexOf('('));

                    new UserMethod(
                    name: name,
                    parameters: null,
                    methodCode: methodScript.ToArray(),
                    container: @class,
                    returnType: ((Keyword.Variable)keywords[(key)key]).type
                    );

                    line += end - line;
                    break;
                }
            }
        }
        public static void Line(string line, UserMethod method)
        {
            List<string> sections = SplitLineIntoSections(line);

            Line newLine = new Line(method);

            for (int i = 0; i < sections.Count; i++)
            {
                string section = sections[i];

                if(section.Contains("("))
                {
                    List<char> seperators = new() { '(', ')', ',' };
                    List<string> parameters = section.Split(seperators.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                    string name = parameters[0];
                    parameters.RemoveAt(0);

                    bool isKey = false;

                    foreach (var key in keywords)
                    {
                        if (name != key.Value.word) continue;

                        if(key.Value is Keyword.Encapsulation)
                        {
                            ((Keyword.Encapsulation)key.Value).Interporate(parameters.ToArray());
                        }

                        isKey = true;
                    }

                    if(isKey) continue;

                    newLine.AddAction(new MethodCall(name, method, parameters.ToArray()));
                } 
                else
                {

                }
                    
            }
        }

        public static void If(object[] parameters)
        {
            if (parameters.Length > 1) return;

            //If statement = new If();
        }

    }
}