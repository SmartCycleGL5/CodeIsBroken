using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Coding.Language
{
    [Serializable]
    public class Method : IVariable
    {
        public string name;

        public Type[] requiredInputTypes = new Type[0];
        public Type returnType;
        [HideInInspector] public Class Class;

        public SerializedDictionary<string, Variable> variables { get; set; } = new();

        string[] methodCode;
        [SerializeField] List<Line> lines = new();

        public Method(string name, Type returnType, string[] methodCode, Class @class)
        {
            this.name = name;
            this.returnType = returnType;
            this.methodCode = methodCode;
            Class = @class;

            InitializeMethod();
        }
        public void InitializeMethod()
        {
            foreach (var line in methodCode)
            {
                bool isMethod = line.Contains("()");

                if (isMethod)
                {
                    lines.Add(new MethodCall(line, this));
                }
            }
        }

        /// <summary>
        /// Try to run the method
        /// </summary>
        /// <param name="input">the input variables</param>
        /// <returns>returns true if it was successful</returns>
        public bool TryRun(Variable[] input = null)
        {
            if (input == null)
            {
                input = new Variable[0];
            }
            if (input.Length == requiredInputTypes.Length)
            {
                for (int i = 0; i < requiredInputTypes.Length; i++)
                {
                    if (input[i].type != requiredInputTypes[i])
                    {
                        variables.Clear();
                        return false;
                    }

                    NewVariable(input[i]);
                }

                Run();
                return true;
            }

            return false;
        }
        void Run()
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
            else if (Class.variables[name] != null)
            {
                return Class.FindVariable(name);
            }

            return null;
        }
    }

    [Serializable, DefaultExecutionOrder(-100),]
    public abstract class Line
    {
        public abstract void Run();
    }
    [Serializable]
    public class MethodCall : Line
    {
        public Method MethodToCall;
        public MethodCall(string line, Method caller)
        {
            MethodToCall = caller.Class.FindMethod(line);
        }

        public override void Run()
        {
            if(!MethodToCall.TryRun())
            {
                Debug.LogError("Could not run method: " + MethodToCall.name);
            }
        }
    }
    [Serializable]
    public class UpdateVariable : Line
    {
        public UpdateVariable()
        {
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }

}