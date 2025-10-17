using System.Collections.Generic;
using UnityEngine;

namespace SharpCube
{
    public interface IKeyword
    {
    }

    public interface IIndentifyer
    {
        public string name { get; set; }
    }
    public interface IContainer : IIndentifyer
    {
        public string[] containedCode { get; set; }

        public Dictionary<string, object> variables { get; set; }
        public Dictionary<string, object> methods { get; set; }

        public void Initialize(string name, string[] code);

        public void AddVariable(string name, object value);
    }
    public class @Class : IContainer, IKeyword
    {
        //IContainer
        public string name { get; set; }
        public string[] containedCode { get; set; }
        public Dictionary<string, object> variables { get; set; }
        public Dictionary<string, object> methods { get; set; }

        public void AddVariable(string name, object value)
        {
            variables.Add(name, value);
        }

        public void Initialize(string name, string[] code)
        {
            this.name = name;
            containedCode = code;
        }
    }
}
