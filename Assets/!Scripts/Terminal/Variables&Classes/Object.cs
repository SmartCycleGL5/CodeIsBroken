using UnityEngine;

namespace Coding.Language
{
    public abstract class Object
    {
        public string name { get; private set; }
        public object value;

        public Object(string name) 
        {
            this.name = name;
        
        }
    }

}