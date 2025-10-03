using AYellowpaper.SerializedCollections;
using Coding;
using NaughtyAttributes;
using NS.RomanLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public static VisualElement canvas { get; private set; }
    public static TabView tabs { get; private set; }
    public static VisualElement windows { get; private set; }

    public static Dictionary<string, Window> OpenWindows { get; private set; } = new();

    RadialFillElement xpIndicator;
    Label levelIndicator;


    Button runButton;

    private void Start()
    {
        Instance = this;

        canvas = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Canvas");
        tabs = canvas.Q<TabView>("Tabs");
        windows = canvas.Q<VisualElement>("Windows");
        xpIndicator = canvas.Q<RadialFillElement>("radial-fill-element");
        levelIndicator = canvas.Q<Label>("Level");
        
        windows.Q<Button>("Close").clicked += CloseCurrentWindow;

        runButton = canvas.Q<Button>("Run");
        runButton.clicked += ScriptManager.ToggleMachines;
        //windows.Q<Button>("Minimize").clicked += DisableWindow;

        DisableWindow();
    }
    private void Update()
    {
        if(ScriptManager.isRunning)
        {
            runButton.text = "Stop";
        } else
        {
            runButton.text = "Run";
        }

        levelIndicator.text = PlayerProgression.Level.ToString();
        xpIndicator.value = PlayerProgression.apparentExperience / PlayerProgression.experienceRequired[PlayerProgression.Level];
    }
    private void OnDestroy()
    {
        canvas.Q<Button>("Run").clicked -= ScriptManager.ToggleMachines;
    }
    [Button]
    public static void CloseCurrentWindow()
    {
        OpenWindows[tabs.activeTab.label].Close();
    }
    [Button]
    public static void CloseAllWindows()
    {
        List<KeyValuePair<string, Window>> windows = OpenWindows.ToList();

        for (int i = windows.Count - 1; i >= 0; i--)
        {
            windows[i].Value.Close();
        }

        OpenWindows.Clear();
    }

    static void EnableWindow()
    {
        windows.visible = true;
        windows.SetEnabled(true);
    }
    static void DisableWindow()
    {
        windows.visible = false;
        windows.SetEnabled(false);
    }
    

    /// <summary>
    /// Dont use this, instead do new UIManager.Window()
    /// </summary>
    /// <param name="window">the window to add</param>
    static void AddWindow(Window window)
    {
        try
        {
            OpenWindows.Add(window.name, window);

            tabs.Add(window.element);

            if (OpenWindows.Count > 0)
            {
                EnableWindow();
            }

            Debug.Log("[UIManager] " + "Added new tab: " + window.name);
        } 
        catch(Exception e)
        {
            window.ForceClose();
            Debug.LogError("[UIManager] " + window.name + ": " + e);
        }
    }
    static void CloseWindow(Window windowToClose)
    {
        OpenWindows.Remove(windowToClose.name);
        tabs.Remove(windowToClose.element);

        if (OpenWindows.Count <= 0)
        {
            DisableWindow();
        }

        Debug.Log("[UIManager] " + "Closed tab: " + windowToClose.name);
    }

    [Button] 
    async void OpenTestWindow()
    {
        VisualTreeAsset element = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.Blue);
        VisualElement blue = element.Instantiate();
        new Window("Blue", blue);
    }

    /// <summary>
    /// Defines window elements
    /// </summary>
    [Serializable]
    public class Window
    {
        public string name;
        public Tab element;
        public IWindow connectedWindow;

        public Window(string name, VisualElement element, IWindow window = null)
        {
            this.name = name;
            this.element = new Tab(name);
            this.element.Add(element);
            this.connectedWindow = window;

            Open();
        }

        void Open()
        {
            AddWindow(this);
        }
        public void Close()
        {
            CloseWindow(this);

            if (connectedWindow != null)
            {
                connectedWindow.Close();
            }
        }
        public void ForceClose()
        {
            if (connectedWindow != null)
            {
                connectedWindow.Close();
            }
        }

        public void Rename(string name)
        {
            CloseWindow(this);

            this.name = name;
            element.name = name;
            element.label = name;

            AddWindow(this);
        }
    }
}
