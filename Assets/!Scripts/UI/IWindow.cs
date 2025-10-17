using UnityEngine;
using static WindowManager;

public interface IWindow
{
    public Window window { get; set; }
    public void Close();
}
