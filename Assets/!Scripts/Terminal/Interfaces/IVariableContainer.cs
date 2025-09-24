using AYellowpaper.SerializedCollections;
using System.Collections.Generic;

namespace Coding.Language
{
    public interface IVariableContainer
    {
        public SerializedDictionary<string, Variable> variables { get; set; }

        /// <summary>
        /// Create a new Variable
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="Type">The type of the variable</param>
        /// <returns>the variable</returns>
        public Variable NewVariable(string name, string value, Type type);
        /// <summary>
        /// Find a variable by name
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns>the variable</returns>
        public Variable FindVariable(string name);
    }
}
