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

        VisualElement terminal;
        
        TextField input;

        Dictionary<string, Button> buttons = new();

        public Window window { get; set; }


        private async void Start()
        {
            if(terminalAsset == null)
            {
                terminalAsset = await Utility.Addressable.ReturnAdressableAsset<VisualTreeAsset>("TerminalUI");
            }

            terminal = terminalAsset.Instantiate();
            window = new Window(machineToEdit.machineCode.name, terminal, this);

            buttons.Add("Close", terminal.Q<Button>("Close"));
            buttons.Add("Save", terminal.Q<Button>("Save"));
            buttons.Add("Run", terminal.Q<Button>("Run"));

            input = terminal.Q<TextField>("Input");

            buttons["Close"].clicked += Close;
            buttons["Save"].clicked += Save;
            buttons["Run"].clicked += RunMachine;

            Load();
        }
        private void OnDestroy()
        {
            buttons["Close"].clicked -= Close;
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
            machineToEdit.Run();
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
