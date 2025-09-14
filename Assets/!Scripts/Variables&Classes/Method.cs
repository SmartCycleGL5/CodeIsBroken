using System;
using UnityEngine;

namespace Coding.Language
{
    [Serializable]
    public abstract class Method
    {
        public string name;

        public Variable[] input = null;
        public Type returnType;
        [HideInInspector] public Class @class;

        public Method(string name, Class @class = null, Type returnType = Type.Void)
        {
            this.name = name;
            this.@class = @class;
            this.returnType = returnType;
        }

        /// <summary>
        /// Try to run the method
        /// </summary>
        /// <param name="input">the input variables</param>
        /// <returns>returns true if it was successful</returns>
        public abstract bool TryRun(Variable[] input = null);
        protected abstract void Run(object[] input);

    }

}