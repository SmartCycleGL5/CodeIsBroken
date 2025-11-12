using Machines;
using ScriptEditor.Console;
using UnityEngine;

public abstract class Machine : MonoBehaviour
{
    public virtual void Reset()
    {

    }

    protected void Print(object debug)
    {
        PlayerConsole.Log(debug);
    }
}     