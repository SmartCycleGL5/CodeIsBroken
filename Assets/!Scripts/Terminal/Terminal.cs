using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UIManager;

namespace Coding
{
    public class Terminal : MonoBehaviour, IWindow
    {
        public BaseMachine machineToEdit { get; private set; }

        public static VisualTreeAsset terminalAsset { get; private set; }
        public static bool findingAsset;

        public static List<Terminal> terminals = new();
        public static bool focused
        {
            get
            {
                if (terminals.Count <= 0) return false;
                foreach (var terminal in terminals)
                {
                    if (terminal.isFocused == null) continue;

                    if ((bool)terminal.isFocused)
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

        public bool? isFocused
        {
            get
            {
                return input == null ? null : input == focusedElement;
            }
        }
        public Window window { get; set; }


        private async void Start()
        {
            Debug.LogError("[Terminal] getting asset");
            if (terminalAsset == null)
            {
                findingAsset = true;
                terminalAsset = await Utility.Addressable.ReturnAdressableAsset<VisualTreeAsset>("Window/Terminal");
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
            input.UnregisterCallback<FocusOutEvent>(OnLoseFocus);
            terminals.Remove(this);
        }
        void OnLoseFocus(FocusOutEvent evt)
        {
            Debug.Log("deselect");
            Save();
        }

        public void Close()
        {
            window.Close();
            Destroy();
        }
        public void Destroy()
        {
            terminals.Remove(this);
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
        public void Save()
        {
            if (machineToEdit == null || ScriptManager.isRunning) return;

            machineToEdit.machineCode.UpdateCode(input.text);
            window.Rename(machineToEdit.machineCode.name);
        }

        public static Terminal NewTerminal(BaseMachine machineScript)
        {
            Terminal newTerminal = machineScript.gameObject.AddComponent<Terminal>();
            newTerminal.SelectMachine(machineScript);
            return newTerminal;
        }
    }
}
