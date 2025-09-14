using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utility;

namespace Coding.Language
{
    using static Interpreter;

    [Serializable, DefaultExecutionOrder(50),]
    /// <summary>
    /// Reperesents Player made classes
    /// </summary>
    public class Class : IVariable, IMethod
    {
        public string name;

        public Class inheritedClass;
        public BaseMachine machine;
        public Dictionary<string, Variable> variables { get; set; } = new();

        public Dictionary<string, UserMethod> methods { get; set; } = new();

        public string[] baseCode;

        #region Class
        public Class(BaseMachine machine, string name, List<string> baseCode, Class inheritedClass = null)
        {
            this.baseCode = baseCode.ToArray();
            this.name = name;
            this.inheritedClass = inheritedClass;
            this.machine = machine;

            InitializeClass();
        }

        /// <summary>
        /// initializes the class
        /// </summary>
        void InitializeClass()
        {
            for (int i = 0; i < baseCode.Length; i++)
            {
                string line = baseCode[i];
                List<string> sections = line.Split(" ").ToList();

                FindAndRetainStrings(ref sections);

                //Find variables & methods
                if (ReturnType(sections[0], out Type type))
                {
                    bool isMethod = sections[1].Contains("()");

                    if (!isMethod && !variables.ContainsKey(sections[1]))
                    {
                        NewVariable(sections[1], type);
                    }
                    else if (isMethod && !methods.ContainsKey(sections[1]))
                    {
                        List<string> methodScript = baseCode.ToList();

                        FindEncapulasion(ref methodScript, i);

                        NewMethod(sections[1], methodScript.ToArray(), type);
                    }
                }


                for (int j = 0; j < sections.Count; j++)
                {
                    //Setting variables
                    if (sections[j] == "=")
                    {
                        Assignment(variables[sections[j - 1]], sections[j + 1]);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Methods
        public Method FindMethod(string name)
        {
            try
            {
                return methods[name];
            }
            catch 
            { 
                if(inheritedClass != null)
                {
                    return inheritedClass.methods[name];
                }
                else
                {
                    return machine.IntegratedMethods[name];
                }
            }
        }
        public void TryRunMethod(string name)
        {
            if (methods.ContainsKey(name))
            {
               FindMethod(name).TryRun();
            }
            else
            {
                Debug.LogWarning("No method of name: " + name);
            }
        }
        public UserMethod NewMethod(string name, string[] code, Type returnType = Type.Void)
        {
            UserMethod method = new UserMethod(name, code, this, returnType);

            methods.Add(name, method);

            return method;
        }
        #endregion

        #region Variables
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
            try
            {
                return variables[name];
            } 
            catch
            {
                return inheritedClass.FindVariable(name);
            }
        }
        #endregion

    }
}

