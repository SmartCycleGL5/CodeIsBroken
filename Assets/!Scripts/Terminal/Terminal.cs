using SharpCube.Highlighting;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static WindowManager;

namespace ScriptEditor
{
    using Console;

    public class Terminal : MonoBehaviour, IWindow
    {
        public SyntaxHighlighting activeHighlighting = new();
        public Script scriptToEdit { get; private set; }

        static VisualTreeAsset terminalAsset;
        public static bool findingAsset;

        public static List<Terminal> terminals = new();
        
        public Window window { get; set; }

        #region UI elements
        
        VisualElement terminal;
        Label inheritedMembers;
        TextField input;
        Label inheritedClass;
        Label console;
        Focusable focusedElement => input.panel.focusController.focusedElement;
         #endregion
        public static bool focused
        {
            get
            {
                if (terminals.Count <= 0) return false;
                foreach (var terminal in terminals)
                {
                    if (terminal.isFocused)
                        return true;
                }
                return false;
            }
        }
        public bool isFocused
        {
            get
            {
                if (input == null) return false;
                try
                {
                    return input == focusedElement;
                }
                catch
                {
                    return false;
                }
            }
        }

        private async void Start()
        {
            activeHighlighting.SetPallate(ColorThemes.Instance.Themes["Default"]);
            
            if (terminalAsset == null)
            {
                Debug.Log("[Terminal] getting asset");

                findingAsset = true;
                terminalAsset = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.Terminal, AddressableToLoad.Object);
                findingAsset = false;
            }
            
            terminal = terminalAsset.Instantiate();
            VisualElement windowElement = terminal.Q<VisualElement>("Window");

#if UNITY_EDITOR
            new Window(scriptToEdit.name, terminal, false, this);
#else
            new Window(scriptToEdit.name, terminal, true, this);
#endif


            windowElement.style.backgroundColor = activeHighlighting.colorPallate.Colors[ColorPallate.Type.backgroundColor];
            windowElement.style.color = activeHighlighting.colorPallate.Colors[ColorPallate.Type.defaultColor];

            inheritedMembers = terminal.Q<Label>("Members");
            inheritedClass = terminal.Q<Label>("InheritedClass");
            input = terminal.Q<TextField>("Input");
            console = terminal.Q<Label>("Output");

            input.RegisterCallback<FocusOutEvent>(OnLoseFocus);

            input.Q<TextElement>().enableRichText = true;

            terminals.Add(this);

            PlayerConsole.LogEvent += ConsoleLog;

            Load();
        }

        private void ConsoleLog(object obj)
        {
            if (obj == "/Clear")
            {
                console.text = "";
                return;
            }
            console.text += obj+ "\n";
        }

        private void OnDestroy()
        {
            PlayerConsole.LogEvent -= ConsoleLog;
            terminals.Remove(this);
            input.UnregisterCallback<FocusOutEvent>(OnLoseFocus);
        }
        void OnLoseFocus(FocusOutEvent evt)
        {
            Save();
        }

        public void Close()
        {
            Save();
            Destroy(this);
        }

        public void Load()
        {
            if (scriptToEdit == null) return;

            Debug.Log(scriptToEdit);

            input.value = scriptToEdit.rawCode;

            inheritedMembers.text = "";

            if (scriptToEdit.connectedMachine != null)
                DisplayIntegratedMethods();
            
            HighlightCode();
            ScriptManager.Compile();
        }
        void DisplayIntegratedMethods()
        {
            inheritedClass.text = scriptToEdit.connectedMachine.toDeriveFrom;
            //availableMethods.text = $"{scriptToEdit.connectedMachine.toDeriveFrom}:\n\n";

            inheritedMembers.text += "Variables:\n";
            
            foreach (var info in scriptToEdit.connectedMachine.variableInfo)
            {
                inheritedMembers.text += info.Name + "\n";

            }
            
            inheritedMembers.text += "\nMethods:\n";

            foreach (var info in scriptToEdit.connectedMachine.methodInfo)
            {
                inheritedMembers.text += info.Name + "(";

                inheritedMembers.text += ")\n";
                
            }
        }

        public void Save()
        {
            if (scriptToEdit == null) return;

            if (ScriptManager.isRunning)
            {
                ScriptManager.StopMachines();
            }

            RemoveHighlight();

            if (scriptToEdit.rawCode != input.text)
            {
                PlayerConsole.Log("Saved!");
                scriptToEdit.Save(input.text); 
            }
            
            //window.Rename(scriptToEdit.name);

            HighlightCode();
        }

        public static Terminal NewTerminal(Script script, Programmable baseMachine = null)
        {
            Terminal newTerminal;
            if (baseMachine == null)
                newTerminal = ScriptManager.instance.gameObject.AddComponent<Terminal>();
            else
            {
                newTerminal = baseMachine.gameObject.AddComponent<Terminal>();
            }

            newTerminal.scriptToEdit = script;

            return newTerminal;
        }

        void HighlightCode()
        {
            input.value = activeHighlighting.HighlightCode(input.text);
        }
        void RemoveHighlight()
        {
            input.value = activeHighlighting.RemoveHighlight(input.text);
        }
    }
}
