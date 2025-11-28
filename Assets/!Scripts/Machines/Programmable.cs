using System;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Reflection;
using CodeIsBroken.UI.Window;
using UnityEngine;
using System.Linq;

[DefaultExecutionOrder(100), DisallowMultipleComponent]
public class Programmable : MonoBehaviour
{
    [InfoBox("The name of the parent class", EInfoBoxType.Warning)]
    public string toDeriveFrom;

    public List<Script> attachedScripts = new();

    public List<FieldInfo> variableInfo = new();
    public List<MethodInfo> methodInfo = new();

    static List<string> disallowedNames = new()
    {
        "",
        //----- Our classes
        "Painter",
        "Assembler",
        "Furnace",
        "Laser",
        "Crane",
        "Machine",
        "MaterialTube",
        "Saw",
        "Console",
        "Material",
        "Random",
        //----- C# key words
        "abstract",
        "as",
        "base",
        "bool",
        "break",
        "byte",
        "case",
        "catch",
        "char",
        "checked",
        "class",
        "const",
        "continue",
        "decimal",
        "default",
        "delegate",
        "do",
        "double",
        "else",
        "enum",
        "event",
        "explicit",
        "extern",
        "false",
        "finally",
        "fixed",
        "float",
        "for",
        "foreach",
        "goto",
        "if",
        "implicit",
        "in",
        "int",
        "interface",
        "internal",
        "is",
        "lock",
        "long",
        "namespace",
        "new",
        "null",
        "object",
        "operator",
        "out",
        "override",
        "params",
        "private",
        "protected",
        "public",
        "readonly",
        "ref",
        "return",
        "sbyte",
        "sealed",
        "short",
        "sizeof",
        "stackalloc",
        "static",
        "string",
        "struct",
        "switch",
        "this",
        "throw",
        "true",
        "try",
        "typeof",
        "uint",
        "ulong",
        "unchecked",
        "unsafe",
        "ushort",
        "using",
        "virtual",
        "void",
        "volatile",
        "while",
    };
    static List<char> disallowedCharacters = new()
    {
        ' ',
        '\t',
        '\n',
        ';',
        ':',
        ',',
        '.',
        '&',
        '@',
        '$',
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '"',
        '#',
        '%',
        '/',
        '=',
        '?',
        '+',
        '-',
        '*',
        '\'',
        '>',
        '<'
    };

    [Button]
    public async void AddScript()
    {
        string name = await WindowManager.OpenEnterValue("Name the script");

        while(!isValidName(name))
        {
            name = await WindowManager.OpenEnterValue("<color=#ff0000>Enter a valid name</color>");
        }
        
        AddScript(new Script(name, toDeriveFrom, this));

        bool isValidName(string name)
        {
            foreach (var item in disallowedNames)
            {
                if (name == item) return false;
            }
            foreach (var item in disallowedCharacters)
            {
                if (name.ToCharArray().Contains(item))
                    return false;
            }
            if(ScriptManager.instance.activePlayerScripts.ContainsKey(name))
            {
                return false;
            }

            return true;
        }
    }
    public virtual void AddScript(Script script)
    {
        script.connectedMachine = this;
        attachedScripts.Add(script);
        
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
