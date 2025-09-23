using AYellowpaper.SerializedCollections;
using System.Collections.Generic;

namespace Coding.Language
{
    public interface IVariable
    {
        public Dictionary<string, Variable> variables { get; set; }

        /// <summary>
        /// Create a new Variable
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="Type">The type of the variable</param>
        /// <returns>the variable</returns>
        public Variable NewVariable(string name, object value);
        /// <summary>
        /// Create a new Variable
        /// </summary>
        /// <param name="variable">The name of the variable</param>
        /// <returns>the variable</returns>
        public Variable NewVariable(Variable variable);
        /// <summary>
        /// Find a variable by name
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns>the variable</returns>
        public Variable FindVariable(string name);
    }
}
