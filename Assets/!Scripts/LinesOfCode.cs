using System;
using UnityEngine;

namespace Coding.Language
{
    [Serializable, DefaultExecutionOrder(-100),]
    public abstract class Line
    {
        public abstract void Run();
    }
    [Serializable]
    public class MethodCall : Line
    {
        public Method MethodToCall;
        public Variable[] input;
        public MethodCall(string line, Class caller, Variable[] input = null)
        {
            MethodToCall = caller.FindMethod(line);
            this.input = input;
        }

        public override void Run()
        {
            try
            {
                MethodToCall.TryRun(input);
            }
            catch (Exception e)
            {
                Debug.LogError("Could not run method: " + MethodToCall.name + "\n\t" + e);
            }
        }
    }

    [Serializable]
    public class UpdateVariable : Line
    {
        public UpdateVariable()
        {
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}