using NaughtyAttributes;
using System.Collections.Generic;
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

        //UI elements
        Dictionary<string, Button> buttons = new();
        VisualElement terminal;
        Label availableMethods;
        TextField input;

        public Window window { get; set; }


        private async void Start()
        {
            if(terminalAsset == null)
            {
                terminalAsset = await Utility.Addressable.ReturnAdressableAsset<VisualTreeAsset>("Window/Terminal");
            }

            terminal = terminalAsset.Instantiate();
            window = new Window(machineToEdit.machineCode.name, terminal, this);

            buttons.Add("Close", terminal.Q<Button>("Close"));
            buttons.Add("Save", terminal.Q<Button>("Save"));
            buttons.Add("Run", terminal.Q<Button>("Run"));

            availableMethods = terminal.Q<Label>("Methods");
            input = terminal.Q<TextField>("Input");

            buttons["Save"].clicked += Save;
            buttons["Run"].clicked += RunMachine;

            Load();
        }
        private void OnDestroy()
        {
            buttons["Save"].clicked -= Save;
            buttons["Run"].clicked -= RunMachine;
            buttons["Run"].clicked -= StopMachine;
        }

        public void Close()
        {
            window.Close();
            Destroy();
        }
        public void Destroy()
        {
            Destroy(this);
        }

        public void RunMachine()
        {
            machineToEdit.RunStart();
            buttons["Run"].text = "Stop";

            buttons["Run"].clicked += StopMachine;
            buttons["Run"].clicked -= RunMachine;
        }
        public void StopMachine()
        {
            machineToEdit.Stop();
            buttons["Run"].text = "Run";

            buttons["Run"].clicked += RunMachine;
            buttons["Run"].clicked -= StopMachine;
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
                availableMethods.text += "\n" + method.Value.name + "()";
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
