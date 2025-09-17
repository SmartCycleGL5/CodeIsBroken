using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Journal
{
    public class JournalManager : MonoBehaviour
    {
        public static JournalManager instance { get; private set; }
        public JournalEntrySO[] machineEntries;
        public JournalEntrySO[] factoryEntries;
        public JournalEntrySO[] economyEntries;
        [SerializeField] string machineTabName = "Machine";
        [SerializeField] string factoryTabName = "Factory";
        [SerializeField] string economyTabName = "Economy";
        public UIDocument journalDoc;
        void Awake()
        {
            if (!instance) { instance = this; }
            else { Destroy(gameObject); }

            journalDoc.rootVisualElement.style.display = DisplayStyle.None;
            machineEntries = Resources.LoadAll<JournalEntrySO>("Journal/Machines");
            factoryEntries = Resources.LoadAll<JournalEntrySO>("Journal/Factory");
            economyEntries = Resources.LoadAll<JournalEntrySO>("Journal/Economy");

        }
        void OnEnable()
        {
            AddEntry(machineEntries, machineTabName);
            AddEntry(factoryEntries, factoryTabName);
            AddEntry(economyEntries, economyTabName);
        }
        void AddEntry(JournalEntrySO[] entries, string nameD)
        {
            List<string> entryNames = entries.Select(obj => obj.name).ToList();
            var tab = journalDoc.rootVisualElement.Q<Tab>(nameD);
            var entryDrop = tab.Q<DropdownField>("Dropdown");
            entryDrop.choices = entryNames;
            entryDrop.RegisterValueChangedCallback(evt => DropDownChange(entries, tab, entryDrop.index));
            entryDrop.index = 0;
        }
        void DropDownChange(JournalEntrySO[] entries, Tab tab, int index)
        {
            var machineImage = tab.Q<VisualElement>("Image");
            var machineExText = tab.Q<Label>("Explanation");
            var machineCodeText = tab.Q<Label>("Code");
            machineImage.style.backgroundImage = entries[index].showcaseI;
            machineExText.text = entries[index].explanation;
            machineCodeText.text = entries[index].codeShowcase;
        }
        public void JournalOnOff()
        {
            if (journalDoc.rootVisualElement.style.display == DisplayStyle.Flex)
            {
                journalDoc.rootVisualElement.style.display = DisplayStyle.None;
            }
            else
            {
                journalDoc.rootVisualElement.style.display = DisplayStyle.Flex;
            }

        }

    }
}