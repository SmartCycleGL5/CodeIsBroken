using UnityEngine;
namespace Coding.Language
{
    public class String : Variable
    {
        public String(string name, object value) : base(name, value)
        {
        }

        public string Get()
        {
            return (string)value;
        }
        public void Set(string value)
        {

        }
    }
}
