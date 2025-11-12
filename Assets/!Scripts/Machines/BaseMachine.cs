using NaughtyAttributes;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[DefaultExecutionOrder(100), DisallowMultipleComponent]
public class BaseMachine : MonoBehaviour
{
    [InfoBox("The name of the monobehaviour class")]
    public string toDeriveFrom;

    public List<Script> attachedScripts = new();

    public List<MethodInfo> methodInfos = new();


    public bool Initialized { get; private set; } = false;

    [Button]
    public void AddScript()
    {
        AddScript(new Script("NewScript" + UnityEngine.Random.Range(1, 100), toDeriveFrom));
    }
    public virtual void AddScript(Script script)
    {
        script.connectedMachine = this;
        attachedScripts.Add(script);
    }

    protected virtual void OnDestroy()
    {
        if (!Initialized) return;
        ScriptManager.instance.RemoveMachine(this);
    }


    [Button]
    public void OpenTerminalForMachine(int script = 0)
    {
        attachedScripts[script].Edit();
    }
    // Why is Torje breaking the code

   void AddMethodsAsIntegrated(System.Type machine)
   {
        methodInfos.Clear();

        foreach (var item in machine.GetMethods())
        {
            if (item.GetBaseDefinition() == item)
            {
                string name = item.Name;
                if (item.IsSpecialName) continue;

                methodInfos.Add(item);
            }
        }
   }
}
