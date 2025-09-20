using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    public static ConveyorManager instance;
    [SerializeField] GridBuilder gridBuilder;
    private void Start()
    {
        instance = this;
    }
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
            Debug.Log("Updated Conveyor");
            conveyor.UpdateConveyor();
        }
    }
}
