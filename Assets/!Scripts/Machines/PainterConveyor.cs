using UnityEngine;

public class PainterConveyor : MonoBehaviour
{
    [SerializeField] Transform input;
    [SerializeField] Transform output;
    [SerializeField] PainterMachine PainterMachine;
    [SerializeField] Item item;


    void Start()
    {
        Tick.OnTick += TakeItem;
    }

    private void TakeItem()
    {
        Debug.Log("TakeItem");
        // Output item
        GameObject outputCell = GridBuilder.instance.LookUpCell(transform.position+transform.forward);
        if(outputCell == null) return;
        if (outputCell.TryGetComponent(out Conveyor conveyorOut))
        {
            // Change after merge.
            if (conveyorOut.item != null) return;
            conveyorOut.item = item;
            item = null;
        }

        //Take in item
        if (item != null) return;
        GameObject inputCell = GridBuilder.instance.LookUpCell(transform.position - transform.forward);
        if(inputCell.TryGetComponent(out Conveyor conveyorIn))
        {
            if(conveyorIn.item == null) return;
            item = conveyorIn.item;
            item.gameObject.transform.position = transform.position;
            conveyorIn.RemoveItem();
        }

        PainterMachine.item = item;
    }

    private void OnDestroy()
    {
        Tick.OnTick -= TakeItem;
    }
}
