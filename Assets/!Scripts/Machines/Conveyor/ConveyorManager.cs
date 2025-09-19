using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    [SerializeField] GridBuilder gridBuilder;
    public static ConveyorManager instance;
    public Conveyor GetConveyor(Vector3 pos)
    {
        GameObject cellObject = GridBuilder.instance.LookUpCell(pos);
        if (cellObject == null)return null;
        if(cellObject.TryGetComponent(out Conveyor conveyor))
        {
            return conveyor;
        }
        Debug.Log("Not found");
        return null;
    }

    public void UpdateCells(Vector3 pos)
    {
        GameObject cellObject = GridBuilder.instance.LookUpCell(pos);
        if (cellObject == null) return;
        if (cellObject.TryGetComponent(out Conveyor conveyor))
        {
            conveyor.UpdateConveyor();
        }
    }
}
