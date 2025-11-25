using UnityEngine;

[RequireComponent(typeof(Programmable))]
public class MachineUIController : MonoBehaviour
{
    [SerializeField] GameObject uiMenu;
    //private Machine machine; machines wont be on objects like this anymore
    bool uiEnabled;

    private void Start()
    {
        uiEnabled = false;
        //machine = GetComponent<Machine>();
    }

    public void ToggleUI(bool toggle)
    {
        //uiEnabled = toggle;
        //uiMenu.SetActive(toggle);
        Debug.Log(uiEnabled);
        TerminalButton();

    }

    public void TerminalButton()
    {

        
        if (gameObject.TryGetComponent(out Programmable machine))
        {
            if(machine.attachedScripts.Count == 0)
                machine.AddScript();
            else
                machine.OpenTerminalForMachine();
        } else
        {
            Debug.Log("[MachineUIController] Couldnt Find Machine");
        }

    }

}
