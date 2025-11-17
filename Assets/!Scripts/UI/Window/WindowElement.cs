using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI.Window
{
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
            WindowManager.AddWindow(this);
        }
        public async void Close()
        {
            if (closing) return;
            closing = true;
    
            if(requestClose)
            {
                if (!await WindowManager.RequestClose(this))
                {
                    closing = false;
                    return;
                }
            }
    
            WindowManager.CloseWindow(this);
    
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
            WindowManager.CloseWindow(this);
    
            this.name = name;
            element.name = name;
            element.label = name;
    
            WindowManager.AddWindow(this);
        }
    }
}
