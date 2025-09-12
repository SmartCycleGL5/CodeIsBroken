using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Terminal
{
    public class Terminal : MonoBehaviour
    {
        [field: SerializeField] public BaseMachine machineToEdit { get; private set; }

        [field: SerializeField] public VisualTreeAsset terminalAsset {  get; private set; }

        VisualElement canvas;
        VisualElement terminal;
        
        TextField input;
        Button saveBTN;
        Button closeBTN;

        private void Start()
        {
            canvas = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Canvas");
            terminal = terminalAsset.Instantiate();
            canvas.Add(terminal);

            input = terminal.Q<TextField>("Input");
            saveBTN = terminal.Q<Button>("Save");
            closeBTN = terminal.Q<Button>("Close");

            saveBTN.clicked += Save;
            closeBTN.clicked += Close;

            SelectMachine(machineToEdit);
        }

        public void Close()
        {
            terminal.RemoveFromHierarchy();
            Destroy(this);
        }

        public void SelectMachine(BaseMachine machineScript)
        {
            machineToEdit = machineScript;

            Load();
        }

        public void Load()
        {
            if (machineToEdit == null) return;

            input.value = machineToEdit.machineCode.Code;
        }
        public void Save()
        {
            if (machineToEdit == null) return;

            machineToEdit.machineCode.Code = input.text;
        }
    }
}
