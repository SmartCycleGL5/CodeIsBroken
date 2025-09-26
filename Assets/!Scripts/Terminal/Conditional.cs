using UnityEngine;

namespace Coding.Language.Conditions
{
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
            }
            else
            {
                @else.Run();
            }
        }
    }
}
