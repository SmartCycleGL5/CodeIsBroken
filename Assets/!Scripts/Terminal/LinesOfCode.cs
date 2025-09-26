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
        public object[] input;
        public MethodCall(string nameOfMethod, Class caller, object[] input)
        {
            Debug.Log("[MethodCall] finding: " + nameOfMethod);

            this.input = input;
            MethodToCall = caller.FindMethod(nameOfMethod);
        }

        public override void Run()
        {
            MethodToCall.TryRun(input);
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
            //throw new NotImplementedException();
        }
    }

    public class Encapsulation : Line
    {
        protected Line[] lines;

        public Encapsulation(Line[] lines)
        {
            this.lines = lines;
        }

        public override void Run()
        {
        }
    }
}