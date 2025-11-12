using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Coding.SharpCube
{
    [Serializable]
    public class IntegratedMethod : Method
    {
        public MethodInfo toCall;
        object toRunFrom;

        public IntegratedMethod(string name, Class @class, ParameterInfo[] parameter, MethodInfo method, object toRunFrom, VariableType returnType = VariableType.Void) : base(name, @class, parameter, returnType)
        {
            toCall = method;
            this.toRunFrom = toRunFrom;
            parameters = toCall.GetParameters();
        }
        public override object TryRun(object[] input)
        {
            return Run(input);
        }

        protected override object Run(object[] input)
        {
            if (input == null)
            {
                Debug.Log("[IntegratedMethod] Running " + toCall.Name);
            }
            else
            {
                Debug.Log("[IntegratedMethod] Running " + toCall.Name + " with the following parameters:");
                foreach (var item in input)
                {
                    Debug.Log("[IntegratedMethod] " + item + " " + item.GetType());
                }
            }
            return toCall.Invoke(toRunFrom, input);
        }
    }
}
