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
        public SerializedDictionary<string, Variable> variables { get; set; } = new();

        [field: SerializeField, SerializedDictionary("Name", "Method")]
        public SerializedDictionary<string, UserMethod> userMethods { get; set; } = new();


        public string[] baseCode;

        public Class(BaseMachine machine, string name, List<string> baseCode, Class inheritedClass = null)
        {
            this.baseCode = baseCode.ToArray();
            this.name = name;
            this.inheritedClass = inheritedClass;
            this.machine = machine;

            machine.Classes.Add(name, this);

            Debug.Log("[Class] New Class: " + name);

            InitializeClass();
        }

        /// <summary>
        /// initializes the class
        /// </summary>
        void InitializeClass()
        {
            Interporate.Variables(this);
            Interporate.Methods(this);
        }


        #region Methods
        public Method GetMethod(string toGet)
        {
            try
            {
                return userMethods[toGet];
            }
            catch
            {
                if (inheritedClass != null)
                {
                    return inheritedClass.GetMethod(toGet);
                }
                else
                {
                    return machine.IntegratedMethods[toGet];
                }
            }
        }

        public void Add(UserMethod toAdd)
        {
            userMethods.Add(toAdd.info.name, toAdd);
        }
        #endregion

        #region Variables
        public Variable GetVariable(string toGet)
        {
            try
            {
                return variables[toGet];
            }
            catch
            {
                return inheritedClass.GetVariable(toGet);
            }
        }
        public void Add(Variable toAdd)
        {
            variables.Add(toAdd.info.name, toAdd);
        }

        #endregion

    }
}

