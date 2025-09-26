using UnityEngine;

public class Laser : Machine
{
    [SerializeField] Transform cell;
    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(MaterialTube));

        base.Initialize(initialClassName);
    }

    public bool Item()
    {
        GameObject cellObj = GridBuilder.instance.LookUpCell(cell.position);
        if(TryGetComponent(out Conveyor conveyor))
        {
            if(conveyor.item != null)
            {
                return true;
            }
        }
        else
        {
            Debug.Log("[Laser] No conveyor found");
        }
        return false;
    }


}
