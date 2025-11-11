using ScriptEditor.Console;
using SharpCube;
using UnityEngine;

public class Machine : MonoBehaviour
{

    public virtual void Reset()
    {

    }

    protected void Print(object debug)
    {
        PlayerConsole.Log(debug);
    }
}
