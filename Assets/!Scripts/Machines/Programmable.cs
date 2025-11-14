using NaughtyAttributes;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[DefaultExecutionOrder(100), DisallowMultipleComponent]
public class Programmable : MonoBehaviour
{
    [InfoBox("The name of the monobehaviour class")]
    public string toDeriveFrom;

    public List<Script> attachedScripts = new();

    public List<FieldInfo> variableInfo = new();
    public List<MethodInfo> methodInfo = new();

    [Button]
    public void AddScript()
    {
        AddScript(new Script("NewScript" + UnityEngine.Random.Range(1, 100), toDeriveFrom, this));
    }
    public virtual void AddScript(Script script)
    {
        script.connectedMachine = this;
        attachedScripts.Add(script);
    }

    protected virtual void OnDestroy()
    {
        ScriptManager.instance.RemoveMachine(this);

        foreach (var script in attachedScripts)
        {
            ScriptManager.instance.activePlayerScripts.Remove(script.name);
        }
    }


    [Button]
    public void OpenTerminalForMachine(int script = 0)
    {
        attachedScripts[script].Edit();
    }
    // Why is Torje breaking the code

   public void AddMethodsAsIntegrated(System.Type machine)
   {
       foreach (var item in machine.GetFields(BindingFlags.Public  | BindingFlags.Instance | BindingFlags.DeclaredOnly))
       {
           if(item.IsSpecialName) continue;
           
           variableInfo.Add(item);
       }
        foreach (var item in machine.GetMethods())
        {
            if (item.GetBaseDefinition() == item)
            {
                if (item.IsSpecialName) continue;
                
                methodInfo.Add(item);
            }
        }
   }
}
