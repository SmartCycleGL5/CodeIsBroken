using UnityEngine;
using UnityEngine.InputSystem;

public class MachineInteraction : MonoBehaviour
{
    [SerializeField] private GridBuilder gridBuilder;
    [SerializeField] private GhostBuilding ghostBuilding;
    private MachineUIController currentMachineUI;

    public void PlayerUpdate()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Looking for object to toggle");
            Vector2 mousePos = GridBuilder.instance.GetGridPosition();
            GameObject building = GridBuilder.instance.LookUpCell(new Vector3(mousePos.x,0,mousePos.y));
            Debug.Log("building: "+building);
            if (building == null) return;

            if (building.TryGetComponent(out MachineUIController machineUI)) 
            {
                Debug.Log(machineUI.gameObject);
                SelectMachine(machineUI);
            }

        }
    }

    private void SelectMachine(MachineUIController machineUI)
    {
        if (currentMachineUI != machineUI) {
            if (currentMachineUI != null) 
            { 
                currentMachineUI.ToggleUI(false); 
            }
            currentMachineUI = machineUI;
            currentMachineUI.ToggleUI(true);
        }
    }
}
