using System;
using System.Reflection;

namespace SharpCube
{
    [Serializable]
    public class IntegratedMethod
    {
        public MethodInfo toCall;
        object toRunFrom;

        public IntegratedMethod(string name, Class @class, ParameterInfo[] parameter, MethodInfo method, object toRunFrom)
        {
            toCall = method;
            this.toRunFrom = toRunFrom;
            //parameters = toCall.GetParameters();
        }
        //public override object TryRun(object[] input)
        //{
        //    return Run(input);
        //}

        protected object Run(object[] input)
        {
            if (input == null)
            {
                UnityEngine.Debug.Log("[IntegratedMethod] Running " + toCall.Name);
            }
            else
            {
                UnityEngine.Debug.Log("[IntegratedMethod] Running " + toCall.Name + " with the following parameters:");
                foreach (var item in input)
                {
                    UnityEngine.Debug.Log("[IntegratedMethod] " + item + " " + item.GetType());
                }
            }
            return toCall.Invoke(toRunFrom, input);
        }
    }
}
