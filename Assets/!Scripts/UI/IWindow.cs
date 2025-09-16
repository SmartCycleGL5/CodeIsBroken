using UnityEngine;
using static UIManager;

public interface IWindow
{
    public Window window { get; set; }
    public void Destroy();
}
