using RoslynCSharp;
using UnityEngine;

public class Domain : MonoBehaviour
{
    public static ScriptDomain ScriptDomain;

    void Start()
    {
        ScriptDomain = new();
    }

}
