using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    [SerializeField] GridBuilder gridBuilder;
    
    public Conveyor GetConveyor(Vector3 pos)
    {
        GameObject cellObject = gridBuilder.LookUpCell(pos);
        if (cellObject == null)return null;
        if(cellObject.TryGetComponent(out Conveyor conveyor))
        {
            return conveyor;
        }
        Debug.Log("Not found");
        return null;
    }
}
