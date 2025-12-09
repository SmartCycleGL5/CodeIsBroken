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
        bool requestClose;
        bool closing;
    
        public WindowElement(string name, bool requestClose = false)
        {
            this.name = name;
            this.requestClose = requestClose;
            
            newTab();
            
            WindowManager.AddWindow(this);
    
            Focus();

            void newTab()
            {
                tab = new Tab(name);
                tab.style.borderTopWidth = new StyleFloat(2);
                tab.style.borderTopColor = new StyleColor(Color.white);
            }
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
            
            Closing();
            closing = false;
        }

        protected virtual void Closing()
        {
            
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
