using System.Collections.Generic;
using CodeIsBroken.UI.Window.CodeEditor;
using ScriptEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

public class TerminalManager : MonoBehaviour
{
    public static List<CodeEditor> editors = new();
    public static VisualTreeAsset terminalUI;
    
    
    public static bool focused
    {
        get
        {
            if (editors.Count <= 0) return false;
            foreach (var terminal in editors)
            {
                if (terminal.isFocused)
                    return true;
            }
            return false;
        }
    }
    async void Start()
    {
        if (terminalUI == null)
        {
            terminalUI = await Addressable.LoadAsset<VisualTreeAsset>("Window/Terminal");
        }
    }
}
