using UnityEngine;
using UnityEngine.InputSystem;

public class MachineInteraction : MonoBehaviour
{
    [SerializeField] private GridBuilder gridBuilder;
    [SerializeField] private GhostBuilding ghostBuilding;
    private MachineUIController currentMachineUI;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Looking for object to toggle");
            GameObject building = gridBuilder.LookUpCell(gridBuilder.GetGridPosition());
            Debug.Log(building);
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
            if (currentMachineUI != null) { currentMachineUI.ToggleUI(false); }
            currentMachineUI = machineUI;
            currentMachineUI.ToggleUI(true);
        }
        if (machineUI==null) { currentMachineUI.ToggleUI(false); }
    }
}
