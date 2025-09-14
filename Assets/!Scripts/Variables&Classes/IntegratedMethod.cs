using System;
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

        public override bool TryRun(object[] input = null)
        {
            Run();
            return true;
        }

        protected override void Run()
        {
            toCall.Invoke(toRunFrom, null);
        }
    }
}
