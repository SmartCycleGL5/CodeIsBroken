using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

namespace Coding.Language
{
    public class UserMethod : Method, IVariable
    {
        string[] methodCode;
        [SerializeField] List<Line> lines = new();

        public Dictionary<string, Variable> variables { get; set; } = new();

        public UserMethod(string name, string[] methodCode, Class @class = null, Type returnType = Type.Void) : base(name, @class, returnType)
        {
            this.methodCode = methodCode;

            InitializeMethod();
        }
        public void InitializeMethod()
        {
            foreach (var line in methodCode)
            {
                bool isMethod = line.Contains("()");

                if (isMethod)
                {
                    lines.Add(new MethodCall(line, @class));
                }
            }
        }

        /// <summary>
        /// Try to run the method
        /// </summary>
        /// <param name="input">the input variables</param>
        /// <returns>returns true if it was successful</returns>
        public override bool TryRun(object[] input = null)
        {
            if (input == null)
            {
                input = new Variable[0];
            }
            if (input.Length == base.input.Length)
            {

                Run();
                return true;
            }

            return false;
        }
        protected override void Run()
        {
            Debug.Log("Running: " + name);

            foreach (var lines in lines)
            {
                lines.Run();
            }

            variables.Clear();
        }
        public Variable NewVariable(string name, Type Type = Type.Bool)
        {
            Variable value = new Variable(name, Type);
            variables.Add(name, value);
            return value;
        }
        public Variable NewVariable(Variable variable)
        {
            variables.Add(variable.name, variable);
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
    }
}

