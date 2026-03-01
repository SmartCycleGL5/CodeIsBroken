using NaughtyAttributes;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[DefaultExecutionOrder(-100), DisallowMultipleComponent]
public class Programmable : MonoBehaviour
{
    [InfoBox("The name of the parent class", EInfoBoxType.Warning)]
    public string toDeriveFrom;

    public List<Script> attachedScripts = new();

    public List<FieldInfo> variableInfo = new();
    public List<MethodInfo> methodInfo = new();
    
    public float inspectorHeight = 1.5f;
    
    public virtual void AddScript(Script script, bool autoOpen = false)
    {
        script.connectedMachine = this;
        attachedScripts.Add(script);

        if (autoOpen)
            script.Edit();
    }

    protected virtual void OnDestroy()
    {
        foreach (var script in attachedScripts)
        {
            script.Delete();
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
