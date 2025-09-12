using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Terminal
{
    public class Terminal : MonoBehaviour
    {
        public BaseMachine machineToEdit { get; private set; }

        public static VisualTreeAsset terminalAsset {  get; private set; }

        VisualElement terminal;
        
        TextField input;
        Button saveBTN;
        Button closeBTN;


        private async void Start()
        {
            if(terminalAsset == null)
            {
                terminalAsset = await Utility.Addressable.ReturnAdressableAsset<VisualTreeAsset>("TerminalUI");
            }

            terminal = terminalAsset.Instantiate();
            UIManager.AddWindow(terminal);

            input = terminal.Q<TextField>("Input");
            saveBTN = terminal.Q<Button>("Save");
            closeBTN = terminal.Q<Button>("Close");

            saveBTN.clicked += Save;
            closeBTN.clicked += Close;

            Load();
        }

        public void Close()
        {
            terminal.RemoveFromHierarchy();
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
        }
        public void Save()
        {
            if (machineToEdit == null) return;

            machineToEdit.machineCode.UpdateCode(input.text);
        }
    }
}
