using System;
using UnityEngine;

namespace Coding.Language
{
    [Serializable]
    public abstract class Method : Object<IMethod>
    {
        public Variable[] input = null;
        [HideInInspector] public Class @class;

        public Method(string name, Class @class, object[] arguments, Type returnType = Type.Void) : base(name, @class, returnType)
        {
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