using NaughtyAttributes;
using System.Collections.Generic;
using Terminal;
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
        foreach (var tab in OpenWindows)
        {
            CloseWindow(tab.Value);
        }
    }

    public static void AddWindow(string name, VisualElement visualElement)
    {
        try
        {
            Tab newTab = new Tab(name);
            newTab.AddToClassList("Tab");
            newTab.Add(visualElement);
            OpenWindows.Add(name, new(name, newTab));

            tabs.Add(newTab);

            Debug.Log("Added new tab: " + name);
        } catch
        {
            Debug.Log(name + "already added");
        }
    }
    public static void CloseWindow(string name)
    {
        CloseWindow(OpenWindows[name]);
    }
    public static void CloseWindow(Window windowToClose)
    {
        windowToClose.Close();
        OpenWindows.Remove(windowToClose.name);
    }
}

public struct Window
{
    public string name;
    public VisualElement element;

    public Window(string name, VisualElement element)
    {
        this.name = name;
        this.element = element;
    }

    public void Close()
    {
        element.RemoveFromHierarchy();
    }
}
