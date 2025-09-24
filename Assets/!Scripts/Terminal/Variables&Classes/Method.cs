using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Coding.Language
{
    [Serializable]
    public abstract class Method
    {
        public string name;
        [HideInInspector] public Class @class;

        public ParameterInfo[] parameters;

        public Method(string name, Class @class, ParameterInfo[] parameters, Type returnType = Type.Void)
        {
            this.name = name;
            this.@class = @class;

            this.parameters = parameters;

            Debug.Log("[Method] New Method: " + this.name);
        }

        /// <summary>
        /// Try to run the method
        /// </summary>
        /// <param name="input">the input variables</param>
        /// <returns>returns true if it was successful</returns>
        public abstract bool TryRun(object[] input = null);
        protected abstract void Run(object[] input);

    }

}