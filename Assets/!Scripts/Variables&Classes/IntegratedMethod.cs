using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Coding.Language
{
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

            foreach (var item in input)
            {
                parameters.Add(item.value);
            }

            Run(parameters.ToArray());
            return true;
        }

        protected override void Run(object[] parameters)
        {
            Debug.Log(toCall.GetParameters()[0]);
            toCall.Invoke(toRunFrom, null);
        }
    }
}
