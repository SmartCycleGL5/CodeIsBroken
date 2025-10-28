using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Journal
{
    public class JournalManager : MonoBehaviour
    {
        public static JournalManager instance { get; private set; }
        public JournalEntrySO[] machineEntries;
        public JournalEntrySO[] HintEntries;
        public JournalEntrySO[] ConceptEntries;
        [SerializeField] string machineTabName = "Machine";
        [SerializeField] string hintsTabName = "Hints";
        [SerializeField] string conceptsTabName = "Concepts";
        private EventCallback<MouseUpEvent> tHintEvent;
        public UIDocument journalDoc;
        public StyleSheet styleSheet;
        private VisualElement windowElement;
        private Label codeT;
        private Label explainT;
        void Awake()
        {
            if (!instance) { instance = this; }
            else { Destroy(gameObject); }
            
            journalDoc.rootVisualElement.style.display = DisplayStyle.None;
            var tabView = journalDoc.rootVisualElement.Q<TabView>();
            tabView.activeTabChanged += TabChange;
            machineEntries = Resources.LoadAll<JournalEntrySO>("Journal/Machines");
            HintEntries = Resources.LoadAll<JournalEntrySO>("Journal/Hints");
            ConceptEntries = Resources.LoadAll<JournalEntrySO>("Journal/Concepts");
            Array.Sort(machineEntries);
            Array.Sort(HintEntries);
            Array.Sort(ConceptEntries);
        }
        void OnEnable()
        {
            AddEntry(machineEntries, machineTabName);
            AddHintEntry(HintEntries, hintsTabName);
            AddEntry(ConceptEntries, conceptsTabName );
        }
        void AddEntry(JournalEntrySO[] entries, string nameD)
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>(nameD);
            var scrollView = tab.Q<ScrollView>("ScrollView");
            foreach (var entry in entries)
            {
                RemoveEmptyEntries(entry);
                if (entry.title != string.Empty && entry.journalTexts.Count > 0 /*&& entry.showcaseI != null*/)
                {
                    var tButton = new Button() { text = entry.title, style = { backgroundImage = entry.showcaseI,} };
                    tButton.AddToClassList("journal-button");
                    RegisterUICallback(tButton, (evt) => ChangeEntry(tab, entry));
                    scrollView?.Add(tButton);
                }
                else
                {
                    Debug.LogError("Missing Information for : " + entry.name + "\nButton Not Added");
                }
            }
        }
        void AddHintEntry(JournalEntrySO[] entries, string nameD)
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>(nameD);
            var hidden = tab.Q<VisualElement>("HideHint");
            var scrollView = tab.Q<ScrollView>();
            var hintText = tab.Q<Label>("HintText");
            hintText.visible = false;
            foreach (var entry in entries)
            {
                RemoveEmptyEntries(entry);
#if UNITY_EDITOR
                entry.bHintTaken = false;
#endif
                if (entry.title != string.Empty && entry.journalTexts.Count > 0)
                {
                    var tButton = new Button() { text = entry.title, style = { backgroundImage = entry.showcaseI} };
                    tButton.AddToClassList("journal-button");
                    RegisterUICallback(tButton, (evt) => Hint(hintText, tab, entry));
                    scrollView?.Add(tButton);
                }
                else
                {
                    Debug.LogError("Missing Information for : " + entry.name + "\nButton Not Added");
                }
            }
        }

        private void Hint(Label hintText, Tab tab, JournalEntrySO entry)
        {
            var scrollView = tab.Q<ScrollView>("ScrollExpla");
            if (scrollView != null) scrollView.verticalScroller.slider.value = 0;
            if(scrollView.childCount >0)
            {scrollView.Clear();}
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
                ChangeEntry(tab, entry, hintText);
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
            if (scrollView != null) scrollView.verticalScroller.slider.value = 0;
            if(scrollView.childCount >0)
            {scrollView.Clear();}
            foreach(var a in entry.journalTexts)
            {
                if (a.style == JournalStyle.explanation)
                {
                    explainT = new();
                    explainT.AddToClassList("explanation_text");
                    explainT.text = a.text;
                    scrollView.Add(explainT);
                }
                else
                {
                    windowElement = new();
                    codeT = new();
                    codeT.selection.isSelectable  = true;
                    windowElement.AddToClassList("Window");
                    codeT.AddToClassList("code_text");
                    codeT.text = a.text;
                    scrollView.Add(windowElement);
                    windowElement.Add(codeT);
                }
            }

        }
        private void ChangeEntry(Tab tab, JournalEntrySO entry, Label hintText)
        {
            ChangeEntry(tab, entry);
            hintText.visible = false;
            entry.bHintTaken = true;
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
        private void TabChange(Tab tab1, Tab tab2)
        {
            var scrollView = tab1.Q<ScrollView>("ScrollExpla");
            if (scrollView != null) scrollView.verticalScroller.slider.value = 0;
            if (scrollView?.childCount > 0)
            { scrollView?.Clear(); }
        }
        private void RemoveEmptyEntries(JournalEntrySO entry)
        {
            entry.journalTexts.RemoveAll(EmptyEntry);
        }
        private static bool EmptyEntry(JournalText journalText)
        {
            return journalText.text == string.Empty;
        }
    }
}