using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Journal
{
    public class JournalManager : MonoBehaviour
    {
        public static JournalManager instance { get; private set; }
        public static List<JournalEntrySO> machineEntries;
        public static List<JournalEntrySO> HintEntries;
        public static List<JournalEntrySO> ConceptEntries;
        [SerializeField] Texture2D[] headerImages;
        [SerializeField] string machineTabName = "Machine";
        [SerializeField] string hintsTabName = "Hints";
        [SerializeField] string conceptsTabName = "Concepts";
        private EventCallback<MouseUpEvent> tHintEvent;
        public UIDocument journalDoc;
        public StyleSheet styleSheet;
        private VisualElement windowElement;
        private Label codeT;
        private Label explainT;
        async void Awake()
        {
            if (!instance) { instance = this; }
            else { Destroy(gameObject); }
            PlayerProgression.onLevelUp += OnLevelUp;
            journalDoc.rootVisualElement.style.display = DisplayStyle.None;
            var tabView = journalDoc.rootVisualElement.Q<TabView>();
            tabView.activeTabChanged += TabChange;
            await GetEntries();
            AddEntries();
            ChangeColorTabHeader(tabView);
        }

        private static async Task GetEntries()
        {
            machineEntries = await Addressable.LoadAssets<JournalEntrySO>("MachineEntries");
            HintEntries = await Addressable.LoadAssets<JournalEntrySO>("HintEntries");
            ConceptEntries = await Addressable.LoadAssets<JournalEntrySO>("ConceptEntries");
            machineEntries.Sort();
            HintEntries.Sort();
            ConceptEntries.Sort();
        }

        private void AddEntries()
        {
            AddEntry(machineEntries, machineTabName);
            AddHintEntry(HintEntries, hintsTabName);
            AddEntry(ConceptEntries, conceptsTabName);
        }
        void ChangeColorTabHeader(TabView tabView)
        {
            for (int i = 0; i < headerImages.Length; i++)
            {
                tabView.GetTabHeader(i).style.backgroundImage = headerImages[i];
                tabView.GetTabHeader(i).style.height = headerImages[i].height;
                tabView.GetTabHeader(i).style.width = headerImages[i].width;
            }
        }

        void AddEntry(List<JournalEntrySO> entries, string nameD)
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>(nameD);
            var scrollView = tab.Q<ScrollView>("ScrollView");
            foreach (var entry in entries)
            {
                RemoveEmptyEntries(entry);
                if (entry.title != string.Empty && entry.journalTexts.Count > 0 /*&& entry.showcaseI != null*/)
                {
                    entry.SetUnlocked();
                    var tButton = new Button()
                    {
                        text = entry.title,
                        style = { backgroundImage = entry.showcaseI, }
                    };
                    tButton.AddToClassList("journal-button");
                    tButton.style.display = entry.IsUnlocked ? DisplayStyle.Flex : DisplayStyle.None;
                    RegisterUICallback(tButton, (evt) => ChangeEntry(tab, entry));
                    scrollView?.Add(tButton);
                }
                else
                {
                    Debug.LogError("Missing Information for : " + entry.name + "\nButton Not Added");
                }
            }
        }
        void AddHintEntry(List<JournalEntrySO> entries, string nameD)
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
                    entry.SetUnlocked();
                    var tButton = new Button() { text = entry.title, style = { backgroundImage = entry.showcaseI} };
                    tButton.AddToClassList("journal-button");
                    tButton.style.display = entry.IsUnlocked ? DisplayStyle.Flex : DisplayStyle.None;
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
            foreach(var text in entry.journalTexts)
            {
                if (text.style == JournalStyle.explanation)
                {
                    explainT = new();
                    explainT.AddToClassList("explanation_text");
                    explainT.text = text.text;
                    scrollView.Add(explainT);
                }
                else
                {
                    windowElement = new();
                    codeT = new();
                    codeT.selection.isSelectable  = true;
                    windowElement.AddToClassList("Window");
                    codeT.AddToClassList("code_text");
                    codeT.text = text.text;
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
            journalDoc.rootVisualElement.style.display = journalDoc.rootVisualElement.style.display != DisplayStyle.None ? DisplayStyle.None : DisplayStyle.Flex;
        }
        void Update()
        {
            //Moved to input reader 
            // if (Keyboard.current.jKey.wasPressedThisFrame)
            // {
            //     JournalOnOff();
            // }
            //Add Button For Leveling up, move it later
            if(Keyboard.current.leftAltKey.isPressed && Keyboard.current.oKey.wasPressedThisFrame)
            {
                PlayerProgression.LevelUp();
            }
        }
        private void TabChange(Tab tab1, Tab tab2)
        {
            var scrollView = tab1.Q<ScrollView>("ScrollExpla");
            if (scrollView != null) scrollView.verticalScroller.slider.value = 0;
            scrollView?.Clear();
            var hidehint = tab2.Q("HideHint");
            if(hidehint != null)hidehint.visible = false;
        }
        private void RemoveEmptyEntries(JournalEntrySO entry)
        {
            entry.journalTexts.RemoveAll(EmptyEntry);
        }
        private static bool EmptyEntry(JournalText journalText)
        {
            return journalText.text == string.Empty;
        }
        void OnLevelUp(int level)
        {
            LevelUnlock(machineEntries, machineTabName);
            LevelUnlock(HintEntries, hintsTabName);
            LevelUnlock(ConceptEntries, conceptsTabName);
        }
        void LevelUnlock(List<JournalEntrySO> entries, string nameTab)
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>(nameTab);
            var scrollView = tab.Q<ScrollView>("ScrollView");
            var buttons = scrollView.Children();
            if (entries.Count != buttons.Count()) return;
            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].SetUnlocked();
                buttons.ElementAt(i).style.display = entries[i].IsUnlocked ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}