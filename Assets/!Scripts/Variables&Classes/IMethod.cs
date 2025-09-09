using AYellowpaper.SerializedCollections;
using UnityEngine;

public interface IMethod
{
    public SerializedDictionary<string, Method> methods { get; set; }

    /// <summary>
    /// Runs a defined method
    /// </summary>
    /// <param name="name">name of the method</param>
    public void TryRunMethod(string name);
    /// <summary>
    /// Creates a new method for this class
    /// </summary>
    /// <param name="name">name of the new method</param>
    /// <param name="code">the source of the method</param>
    /// <param name="returnType">type to return</param>
    /// <returns></returns>
    public Method NewMethod(string name, string[] code, Type returnType = Type.Void);
}
