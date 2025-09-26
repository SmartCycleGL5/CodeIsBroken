using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UIManager;

namespace Coding
{
    public class Terminal : MonoBehaviour, IWindow
    {
        public BaseMachine machineToEdit { get; private set; }

        public static VisualTreeAsset terminalAsset {  get; private set; }
        public static bool findingAsset;

        public static List<Terminal> terminals = new();
        public static bool focused { get {
                foreach (var terminal in terminals)
                {
                    if (terminal.isFocused == null) continue;

                    if((bool)terminal.isFocused)
                        return true;
                }
                return false;
            } }

        //UI elements
        Dictionary<string, Button> buttons = new();
        VisualElement terminal;
        Label availableMethods;
        TextField input;

        public bool? isFocused { get { return input == null ? null : input.panel.focusController.focusedElement == input; } }
        public Window window { get; set; }

        Action runButtonStart { get { return ScriptManager.ToggleMachines; } }
        Action runButtonStop { get { return ScriptManager.ToggleMachines; } }


        private async void Start()
        {
            Debug.LogError("[Terminal] getting asset");
            if (terminalAsset == null )
            {
                findingAsset = true;
                terminalAsset = await Utility.Addressable.ReturnAdressableAsset<VisualTreeAsset>("Window/Terminal");
                findingAsset = false;
            }


            //Debug.LogError("[Terminal] " + terminalAsset);

            terminal = terminalAsset.Instantiate();
            window = new Window(machineToEdit.machineCode.name, terminal, this);

            buttons.Add("Close", terminal.Q<Button>("Close"));
            buttons.Add("Save", terminal.Q<Button>("Save"));
            buttons.Add("Run", terminal.Q<Button>("Run"));

            availableMethods = terminal.Q<Label>("Methods");
            input = terminal.Q<TextField>("Input");

            buttons["Save"].clicked += Save;
            buttons["Run"].clicked += runButtonStart;

            terminals.Add(this);

            Load();
        }
        private void OnDestroy()
        {
            buttons["Save"].clicked -= Save;
            buttons["Run"].clicked -= runButtonStart;
            //buttons["Run"].clicked -= runButtonStop;
        }

        public void Close()
        {
            window.Close();
            Destroy();
        }
        public void Destroy()
        {
            terminals.Remove(this);
            Destroy(this);
        }

        public void RunMachine()
        {
            machineToEdit.RunStart();
            buttons["Run"].text = "Stop";

            //buttons["Run"].clicked += runButtonStop;
            //buttons["Run"].clicked -= runButtonStart;
        }
        public void StopMachine()
        {
            machineToEdit.Stop();
            buttons["Run"].text = "Run";

            //buttons["Run"].clicked += runButtonStart;
            //buttons["Run"].clicked -= runButtonStop;
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
            if (machineToEdit == null) return;

            machineToEdit.machineCode.UpdateCode(input.text);
        }

        public static void NewTerminal(BaseMachine machineScript)
        {
            Terminal newTerminal = Instance.gameObject.AddComponent<Terminal>();
            newTerminal.SelectMachine(machineScript);
        }
    }
}
