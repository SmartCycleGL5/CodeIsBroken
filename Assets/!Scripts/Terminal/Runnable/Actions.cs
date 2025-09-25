using Coding.Language.Lines;
using System;
using UnityEngine;

namespace Coding.Language.Actions
{
    public abstract class Action : IRunnable
    {
        public UserMethod caller { get; set; }

        public Action(UserMethod caller)
        {
            this.caller = caller;
        }

        public abstract void Run();
    }
    public class MethodCall : Action
    {
        public string nameOfMethod;
        public object[] input;
        public MethodCall(string nameOfMethod, UserMethod caller, string[] input = null) : base (caller)
        {
            Debug.Log("[MethodCall] finding: " + nameOfMethod);

            this.nameOfMethod = nameOfMethod;
            this.input = input;
        }

        public override void Run()
        {
            caller.GetMethod(nameOfMethod).TryRun(input);
        }
    }

    public class NewVariable : Action
    {
        ObjectInfo<IVariableContainer> info;

        public NewVariable(UserMethod caller, ObjectInfo<IVariableContainer> info) : base(caller)
        {
            this.info = info;
        }

        public override void Run()
        {
            //new Variable()
        }
    }

    public class UpdateVariable : Action
    {
        public UpdateVariable(UserMethod caller) : base(caller)
        {
        }

        public override void Run()
        {
            //throw new NotImplementedException();
        }
    }
}