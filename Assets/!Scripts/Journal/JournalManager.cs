using CodeIsBroken.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utility;

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
        [SerializeField] private RecipesEntry recipes;
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
            ContractManager.OnNewContract += Contract; //added to make the contact giver not reliant on other systems
            journalDoc.rootVisualElement.style.display = DisplayStyle.None;
            var tabView = journalDoc.rootVisualElement.Q<TabView>();
            tabView.activeTabChanged += TabChange;
            SettingsTabSetBind();
            Contract(null);
            RecipeEntries();
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
            }
        }

        void RecipeEntries()
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>("Recipes");
            var scrollV = tab.Q<ScrollView>();
            recipes.SetUnlocked();
            if (scrollV.childCount <= 0)
            {
                foreach (RecipesText rT in recipes.texts)
                {
                    Label rLabel = new(rT.text);
                    rLabel.AddToClassList("explanation_text");
                    if (!rT.IsUnlocked)
                    {
                        rLabel.style.display = DisplayStyle.None;
                    }
                    scrollV.Add(rLabel);
                }
            }
            else if (scrollV.childCount == recipes.texts.Count)
            {
                List<VisualElement> childS = scrollV.Children().ToList();
                for (int i = 0; i < scrollV.childCount; i++)
                {
                    childS[i].style.display = recipes.texts[i].IsUnlocked ? DisplayStyle.Flex : DisplayStyle.None;
                }
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
                    Button tButton = CreateNewButton(entry);
                    RegisterUICallback(tButton, (evt) => ChangeEntry(tab, entry));
                    scrollView?.Add(tButton);
                }
                else
                {
                    Debug.LogError("Missing Information for : " + entry.name + "\nButton Not Added");
                }
            }
        }
        public void Contract(Contract contract)
        {

            //this should get all the UI for you :))) contract.GetUI()
            var tab = journalDoc.rootVisualElement.Q<Tab>("Contract");
            if (contract == null)
            {
                tab.tabHeader.focusable = false;
                tab.tabHeader.style.display = DisplayStyle.None;
                return;
            }
            tab.tabHeader.focusable = true;
            tab.tabHeader.style.display = DisplayStyle.Flex;
            tab.Clear();
            TemplateContainer contractUI = contract.GetUI();
            contract.onProgress += ProgressAmount;
            contractUI.Children().First().style.width = StyleKeyword.Auto;
            contractUI.Children().First().style.height = StyleKeyword.Auto;
            contractUI.Q<Button>().RemoveFromHierarchy();
            tab.Add(contractUI);
            for (int i = 0; i < contract.requests.Length; i++)
            {
                Contract.Request request = contract.requests[i];
                var rTabView = tab.Q<TabView>("Requests");
                var rTab = rTabView.GetTab(i);
                var rAmount = rTab.Q<Label>("Amount");
                rAmount.text = string.Format("{0}X / {0}X", request.amount);
            }

        }

        private void ProgressAmount()
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>("Contract");
            for (int i = 0; i < ContractManager.ActiveContract.requests.Length; i++)
            {
                Contract.Request request = ContractManager.ActiveContract.requests[i];
                var rTabView = tab.Q<TabView>("Requests");
                var rTab = rTabView.GetTab(i);
                var rAmount = rTab.Q<Label>("Amount");
                rAmount.text = string.Format("{0}X / {1}X", request.amountLeft, request.amount);
            }
        }

        private Button CreateNewButton(JournalEntrySO entry)
        {
            entry.SetUnlocked();
            var tButton = new Button()
            {
                text = string.Empty
            };
            var text = new Label
            {
                text = entry.title
            };
            if (entry.showcaseI)
            {
                text.style.width = 130;
                var image = new VisualElement();
                StyleBackground s = Background.FromSprite(entry.showcaseI);
                image.style.backgroundImage = s;
                image.AddToClassList("journal-button_image");
                tButton.Add(image);
            }
            tButton.AddToClassList("journal-button");
            text.AddToClassList("journal-button_text");

            tButton.Add(text);
            tButton.style.display = entry.IsUnlocked ? DisplayStyle.Flex : DisplayStyle.None;
            return tButton;
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
                    Button tButton = CreateNewButton(entry);
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
            if (scrollView.childCount > 0)
            { scrollView.Clear(); }
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
            if (scrollView.childCount > 0)
            { scrollView.Clear(); }
            foreach (var text in entry.journalTexts)
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
                    codeT.selection.isSelectable = true;
                    windowElement.AddToClassList("Window");
                    codeT.AddToClassList("code_text");
                    codeT.text = text.text;
                    windowElement.Add(codeT);
                    scrollView.Add(windowElement);

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
            if (Keyboard.current.leftAltKey.isPressed && Keyboard.current.oKey.wasPressedThisFrame)
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
            if (hidehint != null) hidehint.visible = false;
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
            RecipeEntries();
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
        void SettingsTabSetBind()
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>("Settings");
            var quitButton = tab.Q<Button>("Quit");
            quitButton.clicked += QuitClicked;
        }

        private void QuitClicked()
        {
#if !UNITY_EDITOR
            Application.Quit()
#endif

        }
    }
}