using UnityEngine;

public class Conveyor : MonoBehaviour, IItemContainer
{
    // Conveyor to send item to next
    public Conveyor nextConveyor;
    public Conveyor recieveFrom;

    [SerializeField] Transform backPos;

    public Item item { get; set; }

    void Start()
    {
        // Check if there is conveyor behind.
        ConveyorManager cm = FindFirstObjectByType<ConveyorManager>();
        Conveyor conveyor = cm.GetConveyor(backPos.position);

        if (conveyor != null)
        {
            conveyor.nextConveyor = this;
            recieveFrom = conveyor;
            Debug.Log(conveyor.name);
        }

        Tick.OnTick += MoveOnTick;
    }

    void MoveOnTick()
    {
        // Checks if last in line
        if (nextConveyor == null && recieveFrom !=null)
        {
            Debug.Log("LastInLine");
            SendItem();
        }
    }

    public void SendItem()
    {
        if(recieveFrom != null)
        {
            if(item == null)
            {
                Debug.Log("SentItem");
                item = recieveFrom.item;
                recieveFrom.item = null;
            }
            recieveFrom.SendItem();
        }
        // T.O. Was here
        if (item != null)
        {
            item.transform.position = transform.position + new Vector3(0, 1, 0);
        }

    }

    public void RemoveItem()
    {
        item = null;
    }

    public bool SetItem(Item item)
    {
        if(item != null) return false;

        this.item = item;
        return true;
    }
}
