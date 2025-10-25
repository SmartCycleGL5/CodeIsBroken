using SharpCube;
using SharpCube.Highlighting;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static WindowManager;

namespace WindowSystem
{
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
                terminalAsset = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.Terminal, AddressableToLoad.GameObject);
                findingAsset = false;
            }


            //Debug.LogError("[Terminal] " + terminalAsset);

            terminal = terminalAsset.Instantiate();
            window = new Window(scriptToEdit.name, terminal, this);

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
            console.text += obj+ "\n";
            
            Debug.Log("Console " + console.text);
        }

        private void OnDestroy()
        {
            PlayerConsole.LogEvent -= ConsoleLog;
            terminals.Remove(this);
            input.UnregisterCallback<FocusOutEvent>(OnLoseFocus);
        }
        void OnLoseFocus(FocusOutEvent evt)
        {
            Debug.Log("deselect");
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

        public bool Save()
        {
            if (scriptToEdit == null) return false;

            if (ScriptManager.isRunning)
            {
                ScriptManager.StopMachines();
            }

            RemoveHighlight();

            if (scriptToEdit.rawCode == input.text)
            {
                Debug.Log("No thing to save");
                HighlightCode();
                return true;
            }

            try
            {
                //console.text = "";
                
                scriptToEdit.Compile(input.text);
                window.Rename(scriptToEdit.name);

                HighlightCode();
                return true;
            }
            catch (Exception e) 
            {
                PlayerConsole.LogWarning("Save failed");
                Debug.LogError(e);
                return false;
            }
            
        }

        public static Terminal NewTerminal(Script script)
        {
            Terminal newTerminal = UIManager.Instance.gameObject.AddComponent<Terminal>();

            newTerminal.scriptToEdit = script;

            return newTerminal;
        }

        void HighlightCode()
        {
            Debug.Log(input.labelElement.enableRichText);
            input.value = SyntaxHighlighting.HighlightCode(input.text);
        }
        void RemoveHighlight()
        {
            Debug.Log(input.labelElement.enableRichText);
            input.value = SyntaxHighlighting.RemoveHighlight(input.text);
        }
    }
}
