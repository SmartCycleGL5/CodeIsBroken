using Unity.VisualScripting;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] ConveyorData conveyorData;
    public GameObject itemHolder;
    [SerializeField] Transform nextPos;
    [SerializeField] Transform prevPos;
    public Conveyor nextInLine;
    bool lastConveyer;


    void Start()
    {
        conveyorData = FindFirstObjectByType<ConveyorData>();
        conveyorData.AddConveyor(new Vector2(transform.position.x, transform.position.z), this);
        Invoke("DetectConveyors", 1);
    }

    void DetectConveyors()
    {
        lastConveyer = false;
        Conveyor prevConveyer = conveyorData.GetConveyorOnCell(new Vector2(nextPos.position.x, nextPos.position.z));
        Debug.Log(prevConveyer);
        if (prevConveyer == null) 
        {
            Debug.Log(conveyorData.GetConveyorOnCell(new Vector2(nextPos.position.x, nextPos.position.z)));
            lastConveyer = true; 
        }
        nextInLine = conveyorData.GetConveyorOnCell(new Vector2(prevPos.position.x, prevPos.position.z));


        if (lastConveyer)
        {
            Debug.Log("Lastconveyer");
            InvokeRepeating(nameof(MoveItem), 2, 2);
        }
    }

    public void MoveItem()
    {
        if(itemHolder == null && nextInLine != null)
        {
            if(nextInLine.itemHolder != null)
            {
                itemHolder = nextInLine.itemHolder;
                nextInLine.itemHolder = null;
            }

            nextInLine.MoveItem();
        }
    }
}
