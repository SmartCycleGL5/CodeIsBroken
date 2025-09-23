using Coding.Language;
using UnityEngine;
namespace Coding.Language
{
    public class Bool : Variable
    {
        public Bool(string name, object value) : base(name, value)
        {
        }
        public bool Get()
        {
            return (bool)value;
        }
        public void Set(bool value)
        {
            
        }
    }
}
