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
        public Script scriptToEdit { get; private set; }

        static VisualTreeAsset terminalAsset;
        public static bool findingAsset;

        public static List<Terminal> terminals = new();
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

        //UI elements
        Dictionary<string, Button> buttons = new();
        VisualElement terminal;
        Label availableMethods;
        TextField input;
        Label console;
        Focusable focusedElement => input.panel.focusController.focusedElement;

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
        public Window window { get; set; }


        private async void Start()
        {
            if (terminalAsset == null)
            {
                Debug.Log("[Terminal] getting asset");

                findingAsset = true;
                terminalAsset = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.Terminal, AddressableToLoad.Object);
                findingAsset = false;
            }


            //Debug.LogError("[Terminal] " + terminalAsset);

            terminal = terminalAsset.Instantiate();
            window = new Window(scriptToEdit.name, terminal, true, this);

            availableMethods = terminal.Q<Label>("Methods");
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

            availableMethods.text = "Available Methods: \n";

            if (scriptToEdit.connectedMachine != null)
                DisplayIntegratedMethods();
            
            HighlightCode();
            ScriptManager.Compile();
        }
        void DisplayIntegratedMethods()
        {
            foreach (var method in scriptToEdit.connectedMachine.IntegratedMethods)
            {
                availableMethods.text += "\n" + /*method.Value.toCall.ReturnType.Name + " " + */method.Value.toCall.Name + "(";
/*
                foreach (var item in method.Value.toCall.GetParameters())
                {
                    availableMethods.text += item.ParameterType.Name + " " + item.Name;
                }*/

                availableMethods.text += ");";
                
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

        public static Terminal NewTerminal(Script script)
        {
            Terminal newTerminal = UIManager.Instance.gameObject.AddComponent<Terminal>();

            newTerminal.scriptToEdit = script;

            return newTerminal;
        }

        void HighlightCode()
        {
            input.value = SyntaxHighlighting.HighlightCode(input.text);
        }
        void RemoveHighlight()
        {
            input.value = SyntaxHighlighting.RemoveHighlight(input.text);
        }
    }
}
