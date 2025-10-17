using Coding;
using NS.RomanLib;
using UnityEngine;
using UnityEngine.UIElements;

[DefaultExecutionOrder(100)]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public static VisualElement canvas { get; private set; }

    RadialFillElement xpIndicator;
    Label levelIndicator;


    Button runButton;

    private void Awake()
    {
        Instance = this;

        canvas = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Canvas");
        xpIndicator = canvas.Q<RadialFillElement>("radial-fill-element");
        levelIndicator = canvas.Q<Label>("Level");

        runButton = canvas.Q<Button>("Run");
        runButton.clicked += ScriptManager.ToggleMachines;
    }
    private void Update()
    {
        if (ScriptManager.isRunning)
        {
            runButton.text = "Stop";
        }
        else
        {
            runButton.text = "Run";
        }

        levelIndicator.text = PlayerProgression.Level.ToString();
        xpIndicator.value = PlayerProgression.apparentExperience / PlayerProgression.experienceRequired[PlayerProgression.Level];
    }
    private void OnDestroy()
    {
        canvas.Q<Button>("Run").clicked -= ScriptManager.ToggleMachines;
    }
}
