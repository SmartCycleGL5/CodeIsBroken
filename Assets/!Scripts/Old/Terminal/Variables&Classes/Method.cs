using System;
using System.Reflection;
using UnityEngine;

namespace Coding.SharpCube
{
    [Serializable]
    public abstract class Method : Object<IMethodContainer>
    {
        public ParameterInfo[] parameters;

        public Method(string name, IMethodContainer container, ParameterInfo[] parameters, VariableType returnType = VariableType.Void) : base(name, container, returnType)
        {
            this.parameters = parameters;

            Debug.Log("[Method] New Method: " + info.name);
        }

        /// <summary>
        /// Try to run the method
        /// </summary>
        /// <param name="input">the input variables</param>
        /// <returns>returns true if it was successful</returns>
        public abstract object TryRun(object[] input = null);
        protected abstract object Run(object[] input);

    }

}