using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Coding.Language
{
    [Serializable]
    public class UserMethod : Method, IVariableContainer, IMethodContainer
    {
        string[] methodCode;
        [SerializeField] List<Line> lines = new();

        public SerializedDictionary<string, Variable> variables { get; set; } = new();
        public SerializedDictionary<string, UserMethod> methods { get; set; } = new();

        public UserMethod(string name, Class @class, ParameterInfo[] parameters, string[] methodCode, Type returnType = Type.Void) : base(name, @class, parameters, returnType)
        {
            this.methodCode = methodCode;

            @class.AddMethod(this);

            InitializeMethod();
        }
        public void InitializeMethod()
        {
            foreach (var line in methodCode)
            {
                lines.Add(ReadLine(line));
            }
        }

        public Line ReadLine(string line)
        {
            List<string> sections = line.Split(" ").ToList();
            Utility.FindAndRetain(ref sections, '"', '"');
            Utility.FindAndRetain(ref sections, '(', ')');

            bool isMethod = line.Contains("(") && line.Contains(")");

            if (isMethod)
            {
                string args = line.Substring(line.IndexOf('('), line.Length - line.IndexOf('('));
                args = args.Replace("(", "");
                args = args.Replace(")", "");
                string name = line.Substring(0, line.IndexOf('('));

                Debug.Log(line);
                Debug.Log(args);


                lines.Add(new MethodCall(name, @class, Interporate.TranslateArguments(args)));
            }
            return null;
        }
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
            Debug.Log("Running: " + name);

            foreach (var line in lines)
            {
                if (line == null) continue;
                line.Run();
            }

            variables.Clear();
        }

        #region Variable
        public Variable NewVariable(string name, string value, Type type)
        {
            Variable variable = null;

            switch (type)
            {
                case Type.Int:
                    {
                        variable = new Int(name, this, int.Parse(value));
                        break;
                    }
                case Type.Float:
                    {
                        variable = new Float(name, this, float.Parse(value));
                        break;
                    }
                case Type.String:
                    {
                        variable = new String(name, this, value);
                        break;
                    }
                case Type.Bool:
                    {
                        variable = new Bool(name, this, bool.Parse(value));
                        break;
                    }
                default:
                    {
                        Debug.LogError("[UserMethod] cannot create variable of type " + type);
                        return null;
                    }
            }

            variables.Add(name, variable);
            return variable;
        }
        public Variable FindVariable(string name)
        {
            if (variables[name] != null)
            {
                return variables[name];
            }
            else if (@class.variables[name] != null)
            {
                return @class.FindVariable(name);
            }

            return null;
        }
        #endregion

        public void AddMethod(UserMethod method)
        {
            throw new NotImplementedException();
        }
    }
}

