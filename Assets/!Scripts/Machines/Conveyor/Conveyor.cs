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
            if(currentItem == null)
            {
                Debug.Log("SentItem");
                currentItem = recieveFrom.currentItem;
                recieveFrom.currentItem = null;
            }
            recieveFrom.SendItem();
        }

        if (currentItem != null)
        {
            currentItem.transform.position = transform.position + new Vector3(0, 1, 0);
        }

    }
}
