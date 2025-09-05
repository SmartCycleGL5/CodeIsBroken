using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] GameObject currentItem;
    public Conveyor nextConveyor;

    [SerializeField] Transform backPos;
    void Start()
    {
        ConveyorManager cm = FindFirstObjectByType<ConveyorManager>();
        Conveyor conveyor = cm.GetConveyor(backPos.position);
        if (nextConveyor == null) return;
        conveyor.nextConveyor = this;
        Debug.Log(nextConveyor.name);
    }

    void MoveOnTick()
    {

    }
}
