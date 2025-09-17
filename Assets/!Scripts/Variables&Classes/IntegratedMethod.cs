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

        public IntegratedMethod(string name, object[] parameter, MethodInfo method, object toRunFrom, Class @class = null, Type returnType = Type.Void) : base(name, parameter, @class, returnType)
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
            try
            {
                if(parameters == null)
                {
                    Debug.Log("[IntegratedMethod] Running " + toCall.Name);
                } else
                {
                    Debug.Log("[IntegratedMethod] Running " + toCall.Name + " with the following parameters:");
                    foreach (var item in parameters)
                    {
                        Debug.Log("[IntegratedMethod] " + item + " " + item.GetType());
                    }
                }
                toCall.Invoke(toRunFrom, parameters);
            }
            catch (Exception ex)
            {
                Debug.LogError("[IntegratedMethod] wrong parameters given \n" + ex);
            }
        }
    }
}
