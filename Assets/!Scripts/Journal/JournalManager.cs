using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Journal
{
    public class JournalManager : MonoBehaviour
    {
        public static JournalManager instance { get; private set; }
        public JournalEntrySO[] machineEntries;
        public JournalEntrySO[] HintEntries;
        public JournalEntrySO[] economyEntries;
        [SerializeField] string machineTabName = "Machine";
        [SerializeField] string hintsTabName = "Hints";
        [SerializeField] string economyTabName = "Economy";
        public UIDocument journalDoc;
        void Awake()
        {
            if (!instance) { instance = this; }
            else { Destroy(gameObject); }

            journalDoc.rootVisualElement.style.display = DisplayStyle.None;
            machineEntries = Resources.LoadAll<JournalEntrySO>("Journal/Machines");
            HintEntries = Resources.LoadAll<JournalEntrySO>("Journal/Hints");
            economyEntries = Resources.LoadAll<JournalEntrySO>("Journal/Economy");

        }
        void OnEnable()
        {
            AddEntry(machineEntries, machineTabName);
            AddEntry(HintEntries, hintsTabName);
            AddEntry(economyEntries, economyTabName);
        }
        void AddEntry(JournalEntrySO[] entries, string nameD)
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>(nameD);
            var scrollView = tab.Q<ScrollView>();
            foreach (var entry in entries)
            {
                if (entry.title != string.Empty && entry.explanation != string.Empty && entry.showcaseI != null)
                {
                    var tButton = new Button() { text = entry.title, style = { backgroundImage = entry.showcaseI } };
                    tButton.RegisterCallback<MouseUpEvent>((evt) => changeEntry(tab, entry));
                    scrollView?.Add(tButton);
                }
                else
                {
                    Debug.LogError("Missing Information for : " + entry.name + "\nButton Not Added");
                }
            }
            // var entryDrop = tab.Q<Dropdown Field>("Dropdown");
            // entryDrop.choices = entryNames;
            // entryDrop.RegisterValueChangedCallback(evt => DropDownChange(entries, tab, entryDrop.index));
            // entryDrop.index = 0;
        }

        private void changeEntry(Tab tab, JournalEntrySO entry)
        {
            var explainT = tab.Q<Label>("Explanation");
            var codeT = tab.Q<Label>("Code");
            explainT.text = entry.explanation;
            codeT.text = entry.codeShowcase;
        }



        // void DropDownChange(JournalEntrySO[] entries, Tab tab, int index)
        // {
        //     var machineImage = tab.Q<VisualElement>("Image");
        //     var machineExText = tab.Q<Label>("Explanation");
        //     var machineCodeText = tab.Q<Label>("Code");
        //     machineImage.style.backgroundImage = entries[index].showcaseI;
        //     machineExText.text = entries[index].explanation;
        //     machineCodeText.text = entries[index].codeShowcase;
        // }
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
        void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                JournalOnOff();
            }
        }
    }
}