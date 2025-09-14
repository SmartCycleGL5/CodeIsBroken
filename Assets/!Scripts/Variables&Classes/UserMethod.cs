using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
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
                lines.Add(ReadLine(line));
            }
        }

        public Line ReadLine(string line)
        {
            List<string> sections = line.Split(" ").ToList();
            Utility.FindAndRetain(ref sections);

            bool isMethod = line.Contains("(") && line.Contains(")");

            if (isMethod)
            {
                lines.Add(new MethodCall(line, @class));
            }
            return null;
        }

        /// <summary>
        /// Try to run the method
        /// </summary>
        /// <param name="input">the input variables</param>
        /// <returns>returns true if it was successful</returns>
        public override bool TryRun(Variable[] input = null)
        {
            if (input == null && this.input == null)
            {
                Run(input);
                return true;
            }
            if (input.Length == base.input.Length)
            {

                Run(input);
                return true;
            }

            return false;
        }
        protected override void Run(object[] input)
        {
            Debug.Log("Running: " + name);

            foreach (var lines in lines)
            {
                lines.Run();
            }

            variables.Clear();
        }

        #region Variable
        public Variable NewVariable(string name, object value)
        {
            Variable variable = new Variable(name, value);
            variables.Add(name, variable);
            return variable;
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
        #endregion
    }
}

