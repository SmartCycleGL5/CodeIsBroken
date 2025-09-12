using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Terminal
{
    public class Terminal : MonoBehaviour
    {
        public static Terminal Instance;

        [field: SerializeField] public BaseMachine machineToEdit { get; private set; }

        public VisualElement ui;
        TextField input;
        Button saveBTN;

        private void Start()
        {
            ui = GetComponent<UIDocument>().rootVisualElement;
            input = ui.Q<TextField>("Input");
            saveBTN = ui.Q<Button>("Save");

            input.value = machineToEdit.machineCode.Code;
            saveBTN.clicked += Save;

            Instance = this;
            SelectMachine(machineToEdit);
        }
        public void SelectMachine(BaseMachine machineScript)
        {
            machineToEdit = machineScript;

            Load();
        }

        [Button]
        public void Load()
        {
            //if (machineToEdit != null)
                //input.text = machineToEdit.machineCode.Code;
        }
        [Button]
        public void Save()
        {
            if (machineToEdit != null)
                machineToEdit.machineCode.Code = input.text;
        }
    }
}
