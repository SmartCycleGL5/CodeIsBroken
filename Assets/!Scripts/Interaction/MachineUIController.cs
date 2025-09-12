using UnityEngine;
using Terminal;

public class MachineUIController : MonoBehaviour
{
    [SerializeField] GameObject uiMenu;
    private MachineScript machine;
    bool uiEnabled;

    private void Start()
    {
        machine = GetComponent<MachineScript>();
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
        Terminal.Terminal terminal = FindFirstObjectByType<Terminal.Terminal>(FindObjectsInactive.Include);
        Debug.Log(terminal);
        terminal.gameObject.SetActive(true);
        terminal.SelectMachine(machine);
    }

}
