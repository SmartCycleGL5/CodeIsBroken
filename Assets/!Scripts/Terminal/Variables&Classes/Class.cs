using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coding.Language
{
    [Serializable, DefaultExecutionOrder(50),]
    /// <summary>
    /// Reperesents Player made classes
    /// </summary>
    public class Class : IVariableContainer, IMethodContainer
    {
        public string name;

        public Class inheritedClass;
        public BaseMachine machine;
        [field: SerializeField, SerializedDictionary("Name", "Variable")]
        public SerializedDictionary<string, IVariable> variables { get; set; } = new();

        [field: SerializeField, SerializedDictionary("Name", "Method")]
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
                Interporate.MethodsAndVariables(baseCode, i, out int end, this);

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
                if (inheritedClass != null)
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
        public IVariable NewVariable(string name, object value, Type type)
        {
            IVariable variable = null;

            switch (type)
            {
                case Type.Void:
                    {
                        Debug.LogError("[Class] cannot create variable of type Void");
                        break;
                    }
                case Type.Int:
                    {
                        variable = new Int(name, this, value);
                        break;
                    }
                case Type.Float:
                    {
                        variable = new Float(name, this, value);
                        break;
                    }
                case Type.String:
                    {
                        variable = new String(name, this, value);
                        break;
                    }
                case Type.Bool:
                    {
                        variable = new Bool(name, this, value);
                        break;
                    }
            }

            variables.Add(name, variable);
            return variable;
        }
        public IVariable FindVariable(string name)
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

