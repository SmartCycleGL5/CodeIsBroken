using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public static VisualElement canvas {  get; private set; }
    public static VisualElement window {  get; private set; }

    private void Start()
    {
        Instance = this;

        canvas = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Canvas");
        window = canvas.Q<VisualElement>("Window");
    }

    public static void AddWindow(VisualElement visualElement)
    {
        window.Add(visualElement);
    }
}
