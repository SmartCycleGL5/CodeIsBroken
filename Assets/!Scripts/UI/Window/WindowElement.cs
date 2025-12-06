using System;
using System.Threading.Tasks;
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
        public Tab tab;
        public IWindow connectedWindow;
        bool requestClose;
        bool closing;
    
        public WindowElement(string name, VisualElement element, bool requestClose = false, IWindow window = null)
        {
            this.name = name;
            this.tab = new Tab(name);
            this.tab.Add(element);
            this.requestClose = requestClose;
            this.connectedWindow = window;
            
            WindowManager.AddWindow(this);
    
            Focus();
        }
    
        public void Focus()
        {
            WindowManager.FocusWindow(this);
        }

        public void ForceClose()
        {
            Close(true);
        }
        public async Task Close(bool overrideRequestClose = false)
        {
            if (closing) return;
            closing = true;
    
            if(requestClose && !overrideRequestClose)
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
    
        public void Rename(string name)
        {
            WindowManager.CloseWindow(this);
    
            this.name = name;
            tab.name = name;
            tab.label = name;
    
            WindowManager.AddWindow(this);
        }
    }
}
