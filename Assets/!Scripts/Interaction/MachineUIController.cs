using UnityEngine;
using Terminal;
using UnityEngine.UI;

public class MachineUIController : MonoBehaviour
{
    [SerializeField] GameObject uiMenu;
    private BaseMachine machine;
    bool uiEnabled;

    private void Start()
    {
        uiEnabled = false;
        machine = GetComponent<BaseMachine>();
    }

    public void ToggleUI(bool toggle)
    {
        uiEnabled = toggle;
        uiMenu.SetActive(toggle);
        Debug.Log(uiEnabled);
        
    }

    private void Update()
    {
        if (!uiEnabled) return;
        uiMenu.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
    }

    public void TerminalButton()
    {
        if (ScriptManager.CreateScript(gameObject, "NewClass" + Random.Range(1000, 9999))) 
        {
            OpenTerminal();
        }
        else { OpenTerminal(); }

    }

    private void OpenTerminal()
    {
        Terminal.Terminal terminal = FindFirstObjectByType<Terminal.Terminal>(FindObjectsInactive.Include);
        Debug.Log(terminal);
        terminal.gameObject.SetActive(true);
        terminal.SelectMachine(gameObject.GetComponent<BaseMachine>());
    }

}
