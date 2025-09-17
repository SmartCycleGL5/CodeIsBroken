using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public GameObject currentItem;

    // Conveyor to send item to next
    public Conveyor nextConveyor;
    public Conveyor recieveFrom;

    [SerializeField] Transform backPos;
    void Start()
    {
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
        if (nextConveyor == null && recieveFrom !=null)
        {
            Debug.Log("LastInLine");
            SendItem();
        }
    }

    public void SendItem()
    {
        if(currentItem == null && recieveFrom != null)
        {
            Debug.Log("SentItem");
            currentItem = recieveFrom.currentItem;
            recieveFrom.currentItem = null;
        }
        if (currentItem != null)
        {
            currentItem.transform.position = transform.position + new Vector3(0, 1, 0);
        }
        recieveFrom.SendItem();
    }
}
