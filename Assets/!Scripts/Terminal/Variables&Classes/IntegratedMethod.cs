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

        public IntegratedMethod(string name, Class @class, ParameterInfo[] parameter, MethodInfo method, object toRunFrom, Type returnType = Type.Void) : base(name, @class, parameter, returnType)
        {
            toCall = method;
            this.toRunFrom = toRunFrom;
        }
        public override bool TryRun(object[] parameters)
        {
            Run(parameters);
            return true;
        }

        protected override void Run(object[] parameters)
        {
            if (parameters == null)
            {
                Debug.Log("[IntegratedMethod] Running " + toCall.Name);
            }
            else
            {
                Debug.Log("[IntegratedMethod] Running " + toCall.Name + " with the following parameters:");
                foreach (var item in parameters)
                {
                    Debug.Log("[IntegratedMethod] " + item + " " + item.GetType());
                }
            }
            toCall.Invoke(toRunFrom, parameters);
        }
    }
}
