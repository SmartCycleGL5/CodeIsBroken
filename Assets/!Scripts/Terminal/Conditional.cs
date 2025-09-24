using UnityEngine;

namespace Coding.Language.Conditions
{
    public abstract class Conditional : Line
    {
        protected bool condition;
        protected Line[] lines;
        public Conditional(bool condtion, Line[] lines)
        {
            this.condition = condtion;
            this.lines = lines;
        }

        public override void Run()
        {
            throw new System.NotImplementedException();
        }
    }

    public class If : Conditional
    {
    }
}
