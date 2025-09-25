using AYellowpaper.SerializedCollections;
using Coding.Language.Lines;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Coding.Language
{
    [Serializable]
    public class UserMethod : Method, IVariableContainer, IMethodContainer
    {
        string[] methodCode;
        [SerializeField] List<IRunnable> lines = new();

        public SerializedDictionary<string, Variable> variables { get; set; } = new();
        public SerializedDictionary<string, UserMethod> userMethods { get; set; } = new();

        public UserMethod(string name, IMethodContainer container, ParameterInfo[] parameters, string[] methodCode, Type returnType = Type.Void) : base(name, container, parameters, returnType)
        {
            this.methodCode = methodCode;
            info.container.Add(this);

            PrepareMethod();
        }
        public void PrepareMethod()
        {
            foreach (var line in methodCode)
            {
                lines.Add(Interporate.Line(line, this));
            }
        }

        //public Line ReadLine(string line)
        //{
        //    List<string> sections = line.Split(" ").ToList();
        //    Utility.FindAndRetain(ref sections, '"', '"');
        //    Utility.FindAndRetain(ref sections, '(', ')');

        //    bool isMethod = line.Contains("(") && line.Contains(")");

        //    if (isMethod)
        //    {
        //        string args = line.Substring(line.IndexOf('('), line.Length - line.IndexOf('('));
        //        args = args.Replace("(", "");
        //        args = args.Replace(")", "");
        //        string name = line.Substring(0, line.IndexOf('('));

        //        Debug.Log(line);
        //        Debug.Log(args);


        //        lines.Add(new MethodCall(name, container, Interporate.TranslateArguments(args)));
        //    }
        //    return null;
        //}
        public override bool TryRun(object[] input = null)
        {
            if (input == null && parameters == null)
            {
                Run(input);
                return true;
            }
            if (input.Length == parameters.Length)
            {

                Run(input);
                return true;
            }

            return false;
        }
        protected override void Run(object[] input)
        {
            Debug.Log("Running: " + info.name);

            foreach (var line in lines)
            {
                if (line == null) continue;
                line.Run();
            }

            variables.Clear();
        }

        #region Variable

        public Variable GetVariable(string toGet)
        {
            if (variables[toGet] != null)
            {
                return variables[toGet];
            }
            else if (info.container.GetType() == typeof(Class))
            {
                Class @class = (Class)info.container;

                if (@class.variables[info.name] == null) return null;

                return @class.GetVariable(toGet);
            }

            return null;
        }

        public void Add(Variable toAdd)
        {
            variables.Add(toAdd.info.name, toAdd);
        }
        #endregion

        public Method GetMethod(string toGet)
        {
            throw new NotImplementedException();
        }
        public void Add(UserMethod toAdd)
        {
            throw new NotImplementedException();
        }
    }
}

