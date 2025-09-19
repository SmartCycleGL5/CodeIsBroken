using UnityEngine;

public class ConveyorItemReciever : MonoBehaviour
{
    [SerializeField] Conveyor conveyor;
    [SerializeField] Transform positionToCheck;

    private void Awake()
    {
        ConveyorCheck();
        Tick.OnTick += RecieveItem;
    }

    void ConveyorCheck()
    {
        GameObject cell = GridBuilder.instance.LookUpCell(positionToCheck.position);
        if (cell == null) return;
        if(cell.TryGetComponent(out Conveyor foundCon))
        {
            conveyor = foundCon;
        }
    }
    public void RecieveItem()
    {
        if (conveyor != null && conveyor.item == null) return;
        
    }
}
