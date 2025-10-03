using UnityEngine;

[DefaultExecutionOrder(69)]
public class PainterConveyor : MonoBehaviour, IItemContainer
{
    
    [SerializeField] Transform input;
    [SerializeField] Transform output;
    [SerializeField] PainterMachine PainterMachine;
    public Item item { get; set; }


    void Start()
    {
        Tick.OnTick += TakeItem;
    }

    private void TakeItem()
    {
        // Output item
        GameObject outputCell = GridBuilder.instance.LookUpCell(transform.position+transform.forward);
        if(outputCell == null) return;
        if (outputCell.TryGetComponent(out Conveyor conveyorOut))
        {
            if (conveyorOut.item != null) return;
            conveyorOut.SetItem(item);
            RemoveItem();
        }

        //Take in item
        if (item != null) return;
        GameObject inputCell = GridBuilder.instance.LookUpCell(transform.position - transform.forward);
        if(inputCell.TryGetComponent(out Conveyor conveyorIn))
        {
            if(conveyorIn.item == null) return;
            SetItem(conveyorIn.item);
            conveyorIn.RemoveItem();
        }

        PainterMachine.item = item;
    }

    public bool RemoveItem(out Item removedItem)
    {
        removedItem = null;
        if (item == null) return false;
        removedItem = item;
        item = null;

        return true;
    }

    public bool SetItem(Item item)
    {
        if (this.item != null) return false;
        this.item = item;
        this.item.transform.position = transform.position+new Vector3(0,1,0);
        return true;
    }
    [DontIntegrate]
    public bool RemoveItem()
    {
        return RemoveItem(out Item item);
    }

    private void OnDestroy()
    { 
        Tick.OnTick -= TakeItem;

        if (item == null) return;
        Debug.Log("Removed item from painter");
        Destroy(item.gameObject);
        RemoveItem();
        
    }
}
