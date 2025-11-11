using Coding.SharpCube.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace Coding.SharpCube.Lines
{
    public interface IRunnable
    {
        public UserMethod caller { get; set; }
        public void Run();
    }

    public class Line : IRunnable
    {
        protected List<Action> actions = new();

        public  UserMethod caller { get; set; }

        public Line(UserMethod caller)
        {
            this.caller = caller;
            caller.AddLine(this);
        }

        public virtual void Run()
        {
            foreach (var action in actions)
            {
                action.Run();
            }
        }

        public void AddAction(IRunnable toadd)
        {
            if (toadd == this) return;

            //actions.Add(toadd);
        }
    }
}
