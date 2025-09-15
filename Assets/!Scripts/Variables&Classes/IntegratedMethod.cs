using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Coding.Language
{
    [Serializable]
    public class IntegratedMethod : Method
    {
        public MethodInfo toCall;
        object toRunFrom;

        public IntegratedMethod(string name, MethodInfo method, object toRunFrom, Class @class = null, Type returnType = Type.Void) : base(name, @class, returnType)
        {
            toCall = method;
            this.toRunFrom = toRunFrom;
        }

        public override bool TryRun(Variable[] input = null)
        {
            List<object> parameters = new();

            if (input != null)
            {
                foreach (var item in input)
                {
                    parameters.Add(item.value);
                }
            }

            Run(parameters.ToArray());
            return true;
        }

        protected override void Run(object[] parameters)
        {
            try
            {
                toCall.Invoke(toRunFrom, new object[] { 5 });
            }
            catch (Exception ex)
            {
                Debug.LogError("wrong parameters given \n" + ex);
            }
        }
    }
}
