using AYellowpaper.SerializedCollections;
using System.Collections.Generic;

namespace Coding.SharpCube
{
    public interface IVariableContainer : IContainer<Variable>
    {
        public SerializedDictionary<string, Variable> variables { get; set; }
        public Variable GetVariable(string toGet);
    }
}
