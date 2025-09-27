using Coding.SharpCube.Actions;
using Coding.SharpCube.Lines;
using UnityEngine;

namespace Coding.SharpCube.Encapsulations
{
    public class Encapsulation : IRunnable
    {
        public UserMethod caller { get; set; }
        protected Line[] lines;

        public Encapsulation(Line[] lines)
        {
            this.lines = lines;
        }

        public virtual void Run()
        {
            throw new System.NotImplementedException();
        }
    }
    public abstract class Conditional : IRunnable
    {
        protected bool condition;
        public UserMethod caller { get; set; }
        public Conditional(bool condition)
        {
            this.condition = condition;
        }

        public abstract void Run();
    }

    public class If : Conditional
    {
        public Encapsulation statement;
        public Encapsulation @else;

        public If(bool condtion, Line[] lines, Encapsulation @else) : base(condtion)
        {
            this.@else = @else;
        }

        public override void Run()
        {
            if (condition)
            {
                statement.Run();
            }
            else
            {
                @else.Run();
            }
        }
    }
}
