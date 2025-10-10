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
            var tabView = journalDoc.rootVisualElement.Q<TabView>();
            tabView.activeTabChanged += TabChange;
            machineEntries = Resources.LoadAll<JournalEntrySO>("Journal/Machines");
            HintEntries = Resources.LoadAll<JournalEntrySO>("Journal/Hints");
            economyEntries = Resources.LoadAll<JournalEntrySO>("Journal/Economy");

        }
        void OnEnable()
        {
            AddEntry(machineEntries, machineTabName);
            AddHintEntry(HintEntries, hintsTabName);
            AddEntry(economyEntries, economyTabName);
        }
        void AddEntry(JournalEntrySO[] entries, string nameD)
        {
            var tab = journalDoc.rootVisualElement.Q<Tab>(nameD);
            var scrollView = tab.Q<ScrollView>("ScrollView");
            foreach (var entry in entries)
            {
                if (entry.title != string.Empty && entry.explanation != string.Empty /*&& entry.showcaseI != null*/)
                {
                    var tButton = new Button() { text = entry.title, style = { backgroundImage = entry.showcaseI, width = 345, height = 135, whiteSpace = WhiteSpace.Pre } };
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
#if UNITY_EDITOR
                entry.bHintTaken = false;
#endif
                if (entry.title != string.Empty && entry.explanation != string.Empty)
                {
                    var tButton = new Button() { text = entry.title, style = { backgroundImage = entry.showcaseI, whiteSpace = WhiteSpace.PreWrap } };
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
            var explainT = tab.Q<Label>("Explanation");
            var codeT = tab.Q<Label>("Code");
            var windowE = tab.Q<VisualElement>("Window");
            explainT.text = string.Empty;
            codeT.text = string.Empty;
            UnRegisterUICallback(hintText, tHintEvent);
            tHintEvent = null;
            windowE.style.visibility = Visibility.Hidden;
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
            var explainT = tab.Q<Label>("Explanation");
            var codeT = tab.Q<Label>("Code");
            var windowE = tab.Q<VisualElement>("Window");
            if (scrollView != null) scrollView.verticalScroller.slider.value = 0;
            explainT.text = entry.explanation;
            TurnCodeVisualsOnOff(entry, codeT, windowE);

        }
        private void ChangeEntry(Tab tab, JournalEntrySO entry, Label hintText)
        {
            ChangeEntry(tab, entry);
            hintText.visible = false;
            entry.bHintTaken = true;
            UnRegisterUICallback(hintText, tHintEvent);
            tHintEvent = null;

        }
        void TurnCodeVisualsOnOff(JournalEntrySO entry, Label codeT, VisualElement windowE)
        {
            if (entry.codeShowcase != string.Empty)
            {
                windowE.style.visibility = Visibility.Visible;
                codeT.text = entry.codeShowcase;
            }
            else
            {
                windowE.style.visibility = Visibility.Hidden;
            }
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
            var explainT = tab1.Q<Label>("Explanation");
            var codeT = tab1.Q<Label>("Code");
            var hintText = tab1.Q<Label>("HintText");
            var windowE = tab1.Q<VisualElement>("Window");
            if (explainT != null) explainT.text = string.Empty;
            if (codeT != null) codeT.text = string.Empty;
            if (hintText != null) hintText.visible = false;
            if (windowE != null) windowE.visible = false;
        }
    }
}