using UnityEngine;

public class TerminalInputBlocker : MonoBehaviour
{
    public static bool IsTyping {get; private set;}

    public void OnInputSelected()
    {
        IsTyping = true;
    }

    public void OnInputUnselected()
    {
        IsTyping = false;
    }
}
