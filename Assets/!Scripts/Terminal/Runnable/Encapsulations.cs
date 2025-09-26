using Coding.Language.Actions;
using Coding.Language.Lines;
using UnityEngine;

namespace Coding.Language.Encapsulations
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
    public abstract class Conditional : Encapsulation
    {
        protected bool condition;
        public Conditional(bool condition, Line[] lines) : base(lines)
        {
            this.condition = condition;
        }
    }

    public class If : Conditional
    {
        public Encapsulation @else;

        public If(bool condtion, Line[] lines, Encapsulation @else) : base(condtion, lines)
        {
            this.@else = @else;
        }

        public override void Run()
        {
            if (condition)
            {
                foreach (var line in lines)
                {
                    line.Run();
                }
            }
            else
            {
                @else.Run();
            }
        }
    }
}
