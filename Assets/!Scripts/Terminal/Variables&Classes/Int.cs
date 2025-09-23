using UnityEngine;
namespace Coding.Language
{
    public class Int : Variable
    {
        public Int(string name, object value) : base(name, value)
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
