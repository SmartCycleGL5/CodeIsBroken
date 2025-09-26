using System;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour, IItemContainer
{
    // Conveyor to send item to next
    public Conveyor nextConveyor;
    public List<Conveyor> recieveFrom;

    [SerializeField] List<Transform> positions;

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
        recieveFrom.Clear();
        ConveyorManager cm = ConveyorManager.instance;
        foreach(var reciever in positions)
        {
            Conveyor conveyor = cm.GetConveyor(reciever.position);
            if (conveyor != null)
            {
                conveyor.nextConveyor = this;
                recieveFrom.Add(conveyor);

                //conveyor.nextConveyor = this;
                //recieveFrom.Add(conveyor);
                //Debug.Log(conveyor.name);
            }
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
            foreach (var conveyor in recieveFrom)
            {
                if (conveyor == null) return;
                if (item == null)
                {
                    //Debug.Log("SentItem");
                    SetItem(conveyor.item);
                    conveyor.RemoveItem();
                }
                conveyor.SendItem();
            }
        }
    }

    private void OnDestroy()
    {
        Tick.OnTick -= MoveOnTick;
        if (item != null)
        {
            Destroy(item.gameObject);
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
