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
        public static VisualElement Popup { get; private set; }
        
        
        public static VisualTreeAsset confirmChoice { get; private set; }
        public static VisualTreeAsset enterValue { get; private set; }
    
        public static Dictionary<string, WindowElement> OpenWindows { get; private set; } = new();

        public static bool popupOpen;
        private async void Start()
        {
            tabs = canvas.Q<TabView>("Tabs");
            windows = canvas.Q<VisualElement>("Windows");
            Popup = canvas.Q<VisualElement>("Popup");
            
            if(confirmChoice == null)
                confirmChoice = await Addressable.LoadAsset<VisualTreeAsset>("UI/Popup/ConfirmChoice");
            if(enterValue == null)
                enterValue = await Addressable.LoadAsset<VisualTreeAsset>("UI/Popup/EnterValue");
            
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
        /// Dont use this, instead do new UIManager.WindowElement()
        /// </summary>
        /// <param name="windowElement">the window to add</param>
        public static void AddWindow(WindowElement windowElement)
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
        public static void CloseWindow(WindowElement windowElementToClose)
        {
            OpenWindows.Remove(windowElementToClose.name);
            tabs.Remove(windowElementToClose.element);
    
            if (OpenWindows.Count <= 0)
            {
                DisableWindow();
            }
    
            Debug.Log("[UIManager] " + "Closed tab: " + windowElementToClose.name);
        }
    
        public static async Task<bool> RequestClose(WindowElement windowElementToClose)
        {
            TemplateContainer current = confirmChoice.Instantiate();
            OpenPopup(current);
            
            Button closeButton = current.Q<Button>("Close");
            Button cancelButton = current.Q<Button>("Cancel");
            Label label = current.Q<Label>();
            
            label.text = $"Are you sure you want to close '{windowElementToClose.name}'?";
            bool requestActive = true;
            bool result = false;
    
            closeButton.clicked += () =>
            {
                requestActive = false;
                result = true;
                ClosePopup(current);
            };
            cancelButton.clicked += () =>
            {
                requestActive = false;
                result = false;
                ClosePopup(current);
            };
    
            while (requestActive)
            {
                await Task.Delay(100);
            }
    
            return result;
        }

        public static async Task<string> OpenEnterValue(string info)
        {
            TemplateContainer current = enterValue.Instantiate();
            OpenPopup(current);

            current.Q<Label>("InfoText").text = info;
            
            bool close = false;

            current.Q<Button>("Confirm").clicked += () =>
            {
                close = true;
                ClosePopup(current);
            };

            while (!close)
            {
                await Task.Delay(100);
            }
            
            return current.Q<TextField>("Input").value;
        }
    
        public static void OpenPopup(TemplateContainer popup)
        {
            Popup.style.visibility = Visibility.Visible;
            popupOpen = true;
            Popup.Add(popup);
        }
        public static void ClosePopup(TemplateContainer popup)
        {
            Popup.style.visibility = Visibility.Hidden;
            popupOpen = false;
            Popup.Remove(popup);
        }
        
        [Button]
        async void OpenTestWindow()
        {
            VisualTreeAsset element = await Addressable.LoadAsset<VisualTreeAsset>("Window/Blue");
            VisualElement blue = element.Instantiate();
            new WindowElement("Blue", blue);
        }
    }

}
