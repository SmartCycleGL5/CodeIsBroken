using AYellowpaper.SerializedCollections;
using Coding.SharpCube.Lines;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace Coding.SharpCube
{
    [Serializable]
    public class UserMethod : Method, IVariableContainer, IMethodContainer
    {
        string[] methodCode;
        [SerializeField] List<IRunnable> lines = new();

        public SerializedDictionary<string, Variable> variables { get; set; } = new();
        public SerializedDictionary<string, UserMethod> userMethods { get; set; } = new();

        public UserMethod(string name, IMethodContainer container, ParameterInfo[] parameters, string[] methodCode, VariableType returnType = VariableType.Void) : base(name, container, parameters, returnType)
        {
            this.methodCode = methodCode;
            info.container.Add(this);

            PrepareMethod();
        }
        public void PrepareMethod()
        {
            lines.Clear();

            foreach (var line in methodCode)
            {
                lines.Add(Interporate.Line(line, this));
            }
        }

        public override object TryRun(object[] input = null)
        {
            Debug.Log("Running: " + info.name);

            try
            {
                return Run(input);
            }
            catch (Exception e)
            {
                Debug.LogError("Couldnt run method becasue of: " + e);
            }

            return null;
        }
        protected override object Run(object[] input)
        {
            foreach (var line in lines)
            {
                if (line == null) continue;
                line.Run();
            }

            variables.Clear();

            return null;
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
            try
            {
                return userMethods[toGet];
            }
            catch
            {
                return info.container.GetMethod(toGet);
            }
        }
        public void Add(UserMethod toAdd)
        {
            userMethods.Add(toAdd.info.name, toAdd);
        }
    }
}

