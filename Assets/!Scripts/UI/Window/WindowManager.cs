using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;
using static CodeIsBroken.UI.UIManager;

namespace CodeIsBroken.UI.Window
{
    public class WindowManager : MonoBehaviour
    {
        public static TabView tabs { get; private set; }
        public static VisualElement windows { get; private set; }
        public static VisualElement confirmChoice { get; private set; }
    
        public static Dictionary<string, WindowElement> OpenWindows { get; private set; } = new();
        private void Start()
        {
            tabs = canvas.Q<TabView>("Tabs");
            windows = canvas.Q<VisualElement>("Windows");
            confirmChoice = canvas.Q<VisualElement>("ConfirmClose");
    
            windows.Q<Button>("Close").clicked += CloseCurrentWindow;
    
            DisableWindow();
    
        }
    
        [Button]
        public static void CloseCurrentWindow()
        {
            OpenWindows[tabs.activeTab.label].Close();
        }
        [Button]
        public static void CloseAllWindows()
        {
            List<KeyValuePair<string, WindowElement>> windows = OpenWindows.ToList();
    
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
        /// <param name="windowElement">the window to add</param>
        static void AddWindow(WindowElement windowElement)
        {
            try
            {
                OpenWindows.Add(windowElement.name, windowElement);
    
                tabs.Add(windowElement.element);
    
                if (OpenWindows.Count > 0)
                {
                    EnableWindow();
                }
    
                Debug.Log("[UIManager] " + "Added new tab: " + windowElement.name);
            }
            catch (Exception e)
            {
                windowElement.ForceClose();
                Debug.LogError("[UIManager] " + windowElement.name + ": " + e);
            }
        }
        static void CloseWindow(WindowElement windowElementToClose)
        {
            OpenWindows.Remove(windowElementToClose.name);
            tabs.Remove(windowElementToClose.element);
    
            if (OpenWindows.Count <= 0)
            {
                DisableWindow();
            }
    
            Debug.Log("[UIManager] " + "Closed tab: " + windowElementToClose.name);
        }
    
        static async Task<bool> RequestClose(WindowElement windowElementToClose)
        {
            Button closeButton = confirmChoice.Q<Button>("Close");
            Button cancelButton = confirmChoice.Q<Button>("Cancel");
            Label label = confirmChoice.Q<Label>();
            
            label.text = $"Are you sure you want to close '{windowElementToClose.name}'?";
            bool requestActive = true;
            bool result = false; //true = success
            toggleChoice(true);
    
            closeButton.clicked += () =>
            {
                requestActive = false;
                result = true;
                toggleChoice(false);
            };
            cancelButton.clicked += () =>
            {
                requestActive = false;
                result = false;
                toggleChoice(false);
            };
    
            while (requestActive)
            {
                await Task.Delay(100);
            }
    
            return result;
    
            void toggleChoice(bool toggle)
            {
                confirmChoice.visible = toggle;
            }
        }
    
        [Button]
        async void OpenTestWindow()
        {
            VisualTreeAsset element = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.Blue);
            VisualElement blue = element.Instantiate();
            new WindowElement("Blue", blue);
        }
    
        /// <summary>
        /// Defines window elements
        /// </summary>
        [Serializable]
        public class WindowElement
        {
            public string name;
            public Tab element;
            public IWindow connectedWindow;
            bool requestClose;
            bool closing;
    
            public WindowElement(string name, VisualElement element, bool requestClose = false, IWindow window = null)
            {
                this.name = name;
                this.element = new Tab(name);
                this.element.Add(element);
                this.requestClose = requestClose;
                this.connectedWindow = window;
    
                Open();
            }
    
            void Open()
            {
                AddWindow(this);
            }
            public async void Close()
            {
                if (closing) return;
                closing = true;
    
                if(requestClose)
                {
                    if (!await RequestClose(this))
                    {
                        closing = false;
                        return;
                    }
                }
    
                CloseWindow(this);
    
                if (connectedWindow != null)
                {
                    connectedWindow.Close();
                }
                closing = false;
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

}
