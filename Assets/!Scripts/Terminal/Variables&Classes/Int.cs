using UnityEngine;
namespace Coding.Language
{
    public class Int : Variable, IGetSetAble<int>
    {
        public Int(string name, IVariable container, object value, Type type = Type.Int) : base(name, container, value, type)
        {
        }

        public int Get()
        {
            return (int)value;
        }
        public void Set(int value)
        {

        }
    }
}
