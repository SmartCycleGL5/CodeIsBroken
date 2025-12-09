using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

namespace CodeIsBroken.UI.Window
{
    public class WindowManager : MonoBehaviour
    {
        public static VisualElement root { get; private set; }
        
        public static TabView tabs { get; private set; }
        public static VisualElement windows { get; private set; }
        public static VisualElement Popup { get; private set; }
        
        
        public static VisualTreeAsset confirmChoice { get; private set; }
        public static VisualTreeAsset enterValue { get; private set; }
    
        public static Dictionary<string, WindowElement> OpenWindows { get; private set; } = new();

        public static bool popupOpen;
        public static bool isFocused => root ==  root.focusController.focusedElement;
        

        private void Awake()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
        }

        private async void Start()
        {
            tabs = root.Q<TabView>("Tabs");
            windows = root.Q<VisualElement>("Windows");
            Popup = root.Q<VisualElement>("Popup");
            
            if(confirmChoice == null)
                confirmChoice = await Addressable.LoadAsset<VisualTreeAsset>("UI/Popup/ConfirmChoice");
            if(enterValue == null)
                enterValue = await Addressable.LoadAsset<VisualTreeAsset>("UI/Popup/EnterValue");
            
            windows.Q<Button>("Close").clicked += CloseAllWindows;
    
            DisableWindow();
    
        }
/*
        private void Update()
        {
            Debug.Log(root.focusController.focusedElement);
        }*/

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
            OpenWindows.Add(windowElement.name, windowElement);
            
            tabs.Add(windowElement.tab);
    
            if (OpenWindows.Count > 0)
            {
                EnableWindow();
            }
    
            Debug.Log("[UIManager] " + "Added new tab: " + windowElement.name);
        }

        #region Close
        [Button]
        public static void CloseCurrentWindow()
        {
            OpenWindows[tabs.activeTab.label].Close();
        }
        [Button]
        public static async void CloseAllWindows()
        {
            Debug.Log("close all windows");
            
            var windows = OpenWindows.ToList();

            for (int i = windows.Count - 1; i >= 0; i--)
            {
                Debug.Log("Closing: "  + windows[i].Value.name);
                await windows[i].Value.Close();
            }
    
            OpenWindows.Clear();
        }
        public static void CloseWindow(WindowElement windowElementToClose)
        {
            OpenWindows.Remove(windowElementToClose.name);
            tabs.Remove(windowElementToClose.tab);
    
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
        #endregion

        public static void FocusWindow(WindowElement windowElement)
        {
            tabs.activeTab = windowElement.tab;
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
    }

}
