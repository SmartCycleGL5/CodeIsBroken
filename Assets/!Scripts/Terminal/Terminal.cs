using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static WindowManager;

namespace Coding
{
    public class Terminal : MonoBehaviour, IWindow
    {
        public BaseMachine machineToEdit { get; private set; }

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
            window = new Window(machineToEdit.machineCode.name, terminal, this);

            availableMethods = terminal.Q<Label>("Methods");
            input = terminal.Q<TextField>("Input");

            input.RegisterCallback<FocusOutEvent>(OnLoseFocus);

            terminals.Add(this);

            Load();
        }

        private void OnDestroy()
        {
            machineToEdit.connectedTerminal = null;
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

        public void SelectMachine(BaseMachine machineScript)
        {
            machineToEdit = machineScript;
        }

        public void Load()
        {
            if (machineToEdit == null) return;

            Debug.Log(machineToEdit.machineCode);

            input.value = machineToEdit.machineCode.Code;

            availableMethods.text = "Available Methods: \n";

            foreach (var method in machineToEdit.IntegratedMethods)
            {
                availableMethods.text += "\n" + method.Value.info.type + " " + method.Value.info.name + "(";

                foreach (var item in method.Value.parameters)
                {
                    availableMethods.text += item.ParameterType.Name + " " + item.Name;
                }

                availableMethods.text += ");";
            }
        }
        public bool Save()
        {
            if (machineToEdit == null || ScriptManager.isRunning) return false;

            try
            {
                machineToEdit.machineCode.Compile(input.text);
                window.Rename(machineToEdit.machineCode.name);
                return true;
            }
            catch
            {
                Debug.LogWarning("[Terminal] Couldnt Save");
                return false;
            }
        }

        public static Terminal NewTerminal(BaseMachine machineScript)
        {
            Terminal newTerminal = machineScript.gameObject.AddComponent<Terminal>();
            newTerminal.SelectMachine(machineScript);
            return newTerminal;
        }
    }
}
