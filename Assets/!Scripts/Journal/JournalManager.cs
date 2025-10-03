using System;
using Coding.SharpCube.Encapsulations;
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
        private EventCallback<MouseUpEvent> tHintEvent;
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
            AddEntry(HintEntries, hintsTabName, "");
            AddEntry(economyEntries, economyTabName);
        }
        void AddEntry(JournalEntrySO[] entries, string nameD)
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>(nameD);
            var scrollView = tab.Q<ScrollView>("ScrollView");
            foreach (var entry in entries)
            {
                if (entry.title != string.Empty && entry.explanation != string.Empty && entry.showcaseI != null)
                {
                    var tButton = new Button() { text = entry.title, style = { backgroundImage = entry.showcaseI } };
                    RegisterUICallback(tButton, (evt) => ChangeEntry(tab, entry));
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
        void AddEntry(JournalEntrySO[] entries, string nameD, string f)
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>(nameD);
            var tabView = journalDoc.rootVisualElement.Q<TabView>();
            var hidden = tab.Q<VisualElement>("HideHint");
            var scrollView = tab.Q<ScrollView>();
            var hintText = tab.Q<Label>("HintText");
            tabView.activeTabChanged += tabChange;
            hintText.visible = false;
            foreach (var entry in entries)
            {
                if (entry.title != string.Empty && entry.explanation != string.Empty)
                {
                    var tButton = new Button() { text = entry.title, style = { backgroundImage = entry.showcaseI } };
                    RegisterUICallback(tButton, (evt) => Hint(hintText, tab, entry, tButton));
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

        private void Hint(Label hintText, Tab tab, JournalEntrySO entry, Button button)
        {
            var explainT = tab.Q<Label>("Explanation");
            var codeT = tab.Q<Label>("Code");
            explainT.text = string.Empty;
            codeT.text = string.Empty;
            UnRegisterUICallback(hintText, tHintEvent);
            tHintEvent = null;
            if (!entry.bHintTaken && tHintEvent == null)
            {
                EventCallback<MouseUpEvent> tCallBack = (evt) => ChangeEntry(tab, entry, hintText);
                hintText.visible = true;
                RegisterUICallback(hintText, tCallBack);
                tHintEvent = tCallBack;
            }
            else if (entry.bHintTaken)
            {
                ChangeEntry(tab, entry, hintText, "");
            }
        }
        private void RegisterUICallback(TextElement uiElement, EventCallback<MouseUpEvent> eventCallback)
        {
            uiElement.RegisterCallback(eventCallback);
        }
        private void UnRegisterUICallback(TextElement uiElement, EventCallback<MouseUpEvent> eventCallback)
        {
            if (eventCallback == null) return;
            uiElement.UnregisterCallback(eventCallback);
        }
        private void ChangeEntry(Tab tab, JournalEntrySO entry)
        {
            var scrollView = tab.Q<ScrollView>("ScrollExpla");
            var explainT = tab.Q<Label>("Explanation");
            var codeT = tab.Q<Label>("Code");
            scrollView.verticalScroller.slider.value = 0;
            explainT.text = entry.explanation;
            codeT.text = entry.codeShowcase;
        }
        private void ChangeEntry(Tab tab, JournalEntrySO entry, Label hintText)
        {
            var explainT = tab.Q<Label>("Explanation");
            var codeT = tab.Q<Label>("Code");
            explainT.text = entry.explanation;
            codeT.text = entry.codeShowcase;
            hintText.visible = false;
            entry.bHintTaken = true;
            UnRegisterUICallback(hintText, tHintEvent);
            tHintEvent = null;
        }
        private void ChangeEntry(Tab tab, JournalEntrySO entry, Label hintText, string p)
        {
            var explainT = tab.Q<Label>("Explanation");
            var codeT = tab.Q<Label>("Code");
            hintText.visible = false;
            explainT.text = entry.explanation;
            codeT.text = entry.codeShowcase;
            UnRegisterUICallback(hintText, tHintEvent);
            tHintEvent = null;
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
        void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                JournalOnOff();
            }
        }
        private void tabChange(Tab tab1, Tab tab2)
        {
            var explainT = tab1.Q<Label>("Explanation");
            var codeT = tab1.Q<Label>("Code");
            var hintText = tab1.Q<Label>("HintText");
            if (explainT != null) explainT.text = string.Empty;
            if(codeT != null)codeT.text = string.Empty;
            if(hintText != null)hintText.visible = false;
        }
    }
}