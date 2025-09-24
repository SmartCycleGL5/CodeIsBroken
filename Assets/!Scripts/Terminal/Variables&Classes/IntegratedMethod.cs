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
        public override bool TryRun(object[] input)
        {
            if(toCall.GetParameters() != parameters) return false;

            Run(input);
            return true;
        }

        protected override void Run(object[] input)
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
            toCall.Invoke(toRunFrom, input);
        }
    }
}
