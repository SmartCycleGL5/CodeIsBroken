using Coding.SharpCube.Lines;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coding.SharpCube.Actions
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
        string nameOfMethod;

        public object @return;
        object[] input;
        public MethodCall(string nameOfMethod, UserMethod caller, object[] input = null) : base (caller)
        {
            List<object> convertedInput = new();

            this.nameOfMethod = nameOfMethod;
            this.input = input;
        }

        public override void Run()
        {
            @return = caller.GetMethod(nameOfMethod).TryRun(input);
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