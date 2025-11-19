using UnityEngine;
using static CodeIsBroken.UI.Window.WindowManager;

namespace CodeIsBroken.UI.Window
{
    public interface IWindow
    {
        public WindowElement window { get; set; }
        public void Close();
    }
}

