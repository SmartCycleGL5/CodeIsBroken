using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Journal
{
    public class JournalManager : MonoBehaviour
    {
        public static JournalManager instance { get; private set; }
        public JournalEntrySO[] machineEntries;
        public UIDocument journalDoc;
        void Awake()
        {
            if (!instance) { instance = this; }
            else { Destroy(gameObject); }
        }
        void OnEnable()
        {   
            machineEntries = Resources.LoadAll<JournalEntrySO>("Journal/Machines");
            List<string> machineNames = machineEntries.Select(obj => obj.name).ToList();
            var machineDrop = journalDoc.rootVisualElement.Q<DropdownField>("MachineChoice");
            machineDrop.choices = machineNames;
            machineDrop.RegisterValueChangedCallback(evt => DropDownChange(machineDrop.index));
            machineDrop.index= 0;
            

        }
        void DropDownChange(int index)
        {
            var machineImage = journalDoc.rootVisualElement.Q<VisualElement>("MachineImage");
            var machineExText = journalDoc.rootVisualElement.Q<Label>("MachineExplanation");
            var machineCodeText = journalDoc.rootVisualElement.Q<Label>("MachineCode");
            machineImage.style.backgroundImage = machineEntries[index].showcaseI;
            machineExText.text = machineEntries[index].explanation;
            machineCodeText.text = machineEntries[index].codeShowcase;
        }
    }
}