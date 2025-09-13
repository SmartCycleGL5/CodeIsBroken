using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using Coding;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public static VisualElement canvas { get; private set; }
    public static TabView tabs { get; private set; }

    public static Dictionary<string, Window> OpenWindows { get; private set; } = new();

    private void Start()
    {
        Instance = this;

        canvas = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Canvas");
        tabs = canvas.Q<TabView>("Tabs");
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

    /// <summary>
    /// Dont use this, instead do new Window()
    /// </summary>
    /// <param name="window">the window to add</param>
    static void AddWindow(Window window)
    {
        try
        {
            OpenWindows.Add(window.name, window);

            tabs.Add(window.element);

            Debug.Log("Added new tab: " + window.name);
        } catch
        {
            Debug.Log(window.name + "already added");
        }
    }
    static void CloseWindow(Window windowToClose)
    {
        OpenWindows.Remove(windowToClose.name);
        tabs.Remove(windowToClose.element);
    }

    public struct Window
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
                connectedWindow.Destroy();
            }
        }
    }
}
