using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MachineInteraction : MonoBehaviour
{
    [SerializeField] private GridBuilder gridBuilder;
    [SerializeField] private GhostBuilding ghostBuilding;
    private MachineUIController currentMachineUI;

    public void PlayerUpdate()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("[MachineInteration] Looking for object to toggle");
            // Vector2 mousePos = GridBuilder.instance.GetGridPosition();
            // GameObject building = GridBuilder.instance.LookUpCell(new Vector3(mousePos.x,0,mousePos.y));
            // if (building == null)
            // {
            //     // If mouse clicks on empty space, and it is not the menu panel, close it.
            //     if (currentMachineUI != null && !EventSystem.current.IsPointerOverGameObject())
            //     { 
            //         currentMachineUI.ToggleUI(false);
            //         currentMachineUI = null;
            //     }
            //     return;
            // }
            if(EventSystem.current.IsPointerOverGameObject()) return;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                GameObject building = GridBuilder.instance.LookUpCell(hit.collider.gameObject.transform.position);
                if (building == null)
                {
                    // If mouse clicks on empty space, and it is not the menu panel, close it.
                    if (currentMachineUI != null && !EventSystem.current.IsPointerOverGameObject())
                    { 
                        //currentMachineUI.ToggleUI(false);
                        currentMachineUI = null;
                    }
                    return;
                }
                if (building.TryGetComponent(out MachineUIController machineUI)) 
                {
                    SelectMachine(machineUI);
                    Debug.Log("[MachineInteration] building: " + building);
                }
            }
        }
    }

    private void SelectMachine(MachineUIController machineUI)
    {
        machineUI.ToggleUI(true);
        //currentMachineUI.TerminalButton();

    }
}
