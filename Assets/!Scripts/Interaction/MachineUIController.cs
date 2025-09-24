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
        //uiMenu.SetActive(toggle);
        Debug.Log(uiEnabled);
        TerminalButton();

    }

    private void Update()
    {
        if (!uiEnabled) return;
        uiMenu.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
    }

    public void TerminalButton()
    {
        Debug.LogError("[MachineUIController] Open Terminal plz");

        if (gameObject.TryGetComponent(out BaseMachine machine))
        {
            Debug.LogError("[MachineUIController] " + machine + " is " + machine.Initialized);

            //if (machine.Initialized == false)
                machine.Initialize("NewClass" + Random.Range(100, 1000));

            Debug.LogError("[MachineUIController] opening for " + machine);
            machine.OpenTerminalForMachine();
        } else
        {
            Debug.LogError("[MachineUIController] Couldnt Find Machine");
        }

    }

}
