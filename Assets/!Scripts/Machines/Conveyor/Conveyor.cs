using UnityEngine;

public class Conveyor : MonoBehaviour, IItemContainer
{
    // Conveyor to send item to next
    public Conveyor nextConveyor;
    public Conveyor recieveFrom;

    [SerializeField] Transform backPos;

    public Item item { get; set; }

    Vector3 itemPosition { get { return item.transform.position = transform.position + new Vector3(0, 1, 0);  } }

    void Start()
    {
        UpdateConveyor();

        ConveyorManager.instance.UpdateCells(transform.position+transform.forward);

        Tick.OnTick += MoveOnTick;
    }

    public void UpdateConveyor()
    {
        ConveyorManager cm = ConveyorManager.instance;
        Conveyor conveyor = cm.GetConveyor(backPos.position);
        if (conveyor != null)
        {
            conveyor.nextConveyor = this;
            recieveFrom = conveyor;
            Debug.Log(conveyor.name);
        }
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
                SetItem(recieveFrom.item);
                recieveFrom.RemoveItem();
            }
            recieveFrom.SendItem();
        }
        // T.O. Was here
        if (item != null)
        {
            //item.transform.position = itemPosition;
        }

    }

    public bool RemoveItem(out Item removedItem)
    {
        removedItem = null;
        if(item == null) return false;
        removedItem = item;
        item = null;

        return true;
    }

    public bool SetItem(Item item)
    {
        if (item == null) return false;
        if(this.item != null) return false;

        this.item = item;
        this.item.gameObject.transform.position = itemPosition;
        return true;
    }
    [DontIntegrate]
    public bool RemoveItem()
    {
        return RemoveItem(out Item item);
    }
}
