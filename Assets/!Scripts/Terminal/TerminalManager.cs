using UnityEngine;
using UnityEngine.UIElements;
using Utility;

public class TerminalManager : MonoBehaviour
{
    public static VisualTreeAsset terminalUI;
    async void Start()
    {
        if (terminalUI == null)
        {
            terminalUI = await Addressable.LoadAsset<VisualTreeAsset>("Window/Terminal");
        }
    }
}
