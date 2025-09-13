using AYellowpaper.SerializedCollections;

namespace Coding.Language
{
    public enum Type
    {
        Void,
        Float,
        Int,
        String,
        Bool
    }
    public interface IVariable
    {
        public SerializedDictionary<string, Variable> variables { get; set; }

        /// <summary>
        /// Create a new Variable
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="Type">The type of the variable</param>
        /// <returns>the variable</returns>
        public Variable NewVariable(string name, Type Type = Type.Bool);
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
