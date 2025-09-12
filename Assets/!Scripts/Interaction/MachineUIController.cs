using UnityEngine;
using Terminal;

public class MachineUIController : MonoBehaviour
{
    [SerializeField] GameObject uiMenu;
    [SerializeField] MachineScript machine;
    bool uiEnabled;
    
    public void ToggleUI(bool toggle)
    {
        uiEnabled = toggle;
        uiMenu.SetActive(toggle);
        Debug.Log(uiEnabled);
        
    }

    private void Update()
    {
        if (!uiEnabled) return;
        //uiMenu.transform.LookAt(new Vector3(0, Camera.main.transform.position.y,0));
    }

    public void TerminalButton()
    {
        Terminal.Terminal terminal = FindFirstObjectByType<Terminal.Terminal>(FindObjectsInactive.Include);
        Debug.Log(terminal);
        terminal.gameObject.SetActive(true);
        terminal.SelectMachine(machine);
    }

}
