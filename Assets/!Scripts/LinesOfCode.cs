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
        public MethodCall(string line, Class caller)
        {
            MethodToCall = caller.FindMethod(line);
        }

        public override void Run()
        {
            try
            {
                MethodToCall.TryRun();
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