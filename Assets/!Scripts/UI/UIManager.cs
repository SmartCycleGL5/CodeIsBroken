using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI
{
    [DefaultExecutionOrder(100)]
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
    
        public static VisualElement canvas { get; private set; }
        
        private void Awake()
        {
            Instance = this;
    
            canvas = GetComponent<UIDocument>().rootVisualElement;
    /*
            runButton = canvas.Q<Button>("Run");
            runButton.clicked += ScriptManager.ToggleMachines;*/
        }
        
        /*
        Label levelIndicator;
    
    
        Button runButton;
    
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
            
        }
        private void OnDestroy()
        {
            canvas.Q<Button>("Run").clicked -= ScriptManager.ToggleMachines;
        }*/
    }
}

