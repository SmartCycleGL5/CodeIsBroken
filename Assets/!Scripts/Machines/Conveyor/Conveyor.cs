using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public GameObject currentItem;
    public Conveyor nextConveyor;

    [SerializeField] Transform backPos;
    void Start()
    {
        ConveyorManager cm = FindFirstObjectByType<ConveyorManager>();
        Conveyor conveyor = cm.GetConveyor(backPos.position);
        if (conveyor == null) return;
        conveyor.nextConveyor = this;
        Debug.Log(conveyor.name);
        InvokeRepeating("MoveOnTick", 1,2);
    }

    void MoveOnTick()
    {
        Debug.Log("MOveOnTick"+currentItem+" "+nextConveyor);
        if (nextConveyor != null && nextConveyor.currentItem == null && currentItem != null)
        {
            nextConveyor.currentItem = currentItem;
            currentItem = null;
            Debug.Log("MovedItem");
        }
    }
}
