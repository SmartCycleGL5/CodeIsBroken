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
        protected List<IRunnable> actions = new();

        public  UserMethod caller { get; set; }

        public virtual void Run()
        {
            foreach (var action in actions)
            {
                action.Run();
            }
        }

        public void Add(IRunnable toadd)
        {
            if (toadd == this) return;

            actions.Add(toadd);
        }
    }
}
