using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utility;

namespace Coding.Language
{
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

        [SerializedDictionary("Name", "Method")]
        public SerializedDictionary<string, UserMethod> methods { get; set; } = new();

        public string[] baseCode;

        #region Class
        public Class(BaseMachine machine, string name, List<string> baseCode, Class inheritedClass = null)
        {
            this.baseCode = baseCode.ToArray();
            this.name = name;
            this.inheritedClass = inheritedClass;
            this.machine = machine;

            Debug.Log("[Class] New Class: " + name);

            InitializeClass();
        }

        /// <summary>
        /// initializes the class
        /// </summary>
        void InitializeClass()
        {
            for (int i = 0; i < baseCode.Length; i++)
            {
                Interpreter.DefineMethodsAndVariables(baseCode, i, out int end, this);

                i += end - i; //skips until after the end of the method
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

        public void AddMethod(UserMethod method)
        {
            methods.Add(method.name, method);
        }
        #endregion

        #region Variables
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

