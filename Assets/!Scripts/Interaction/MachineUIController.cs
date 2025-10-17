using UnityEngine;
using Coding;
using UnityEngine.UI;

public class MachineUIController : MonoBehaviour
{
    [SerializeField] GameObject uiMenu;
    private Machine machine;
    bool uiEnabled;

    private void Start()
    {
        uiEnabled = false;
        machine = GetComponent<Machine>();
    }

    public void ToggleUI(bool toggle)
    {
        uiEnabled = toggle;
        uiMenu.SetActive(toggle);
        Debug.Log(uiEnabled);
        //TerminalButton();

    }

    public void TerminalButton()
    {

        
        if (gameObject.TryGetComponent(out BaseMachine machine))
        {
            if(!machine.Initialized)
                machine.Initialize("NewClass" + UnityEngine.Random.Range(1000, 9999));

            
            machine.OpenTerminalForMachine();
        } else
        {
            Debug.Log("[MachineUIController] Couldnt Find Machine");
        }

    }

}
