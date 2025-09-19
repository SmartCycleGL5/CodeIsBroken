using System.Threading.Tasks;
using UnityEngine;

public class CraneMachine : Machine//, IItemContainer
{
    [Space(10), SerializeField] Transform piviot;
    [SerializeField] Transform grabLocation;
    [SerializeField] Transform holdLocation;

    public Item item { get; set; }

    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(CraneMachine));

        base.Initialize(initialClassName);
    }

    public async void Rotate(int degrees)
    {
        float timer = 0;
        degrees *= 90;
        Vector3 startRot = piviot.eulerAngles;

        float timeToFinish = Tick.tickLength * .5f;

        while (isRunning && timer < timeToFinish)
        {
            piviot.Rotate(0, (degrees * Time.deltaTime) / timeToFinish, 0);

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000 * timeToFinish));
            timer += Time.deltaTime;
        }

        piviot.transform.eulerAngles = startRot + Vector3.up * degrees;
    }

    public void GrabLoseItem()
    {
        GameObject cell = GridBuilder.instance.LookUpCell(grabLocation.position);

        if (cell == null)
        {
            Debug.Log("[Crane] Nothing in cell");
            return;
        }

        if (!cell.TryGetComponent(out Conveyor conveyor))
        {
            Debug.Log("[Crane] Cell not conveyor");
            return;
        }

        if (item == null)
        {
            Debug.Log("[Crane] Grab");
            if (conveyor.item == null)
                return;

            SetItem(conveyor.item);
            conveyor.RemoveItem();
        }
        else if (conveyor.SetItem(item))
        {
            RemoveItem();
        }

    }
    [IgnoreMethod]
    public bool SetItem(Item item)
    {

        if (item != null) return false;

        this.item = item;
        this.item.transform.parent = holdLocation;
        return true;
    }
    [IgnoreMethod]
    public void RemoveItem()
    {
        if (item == null) return;
        item.transform.parent = null;
        item = null;
    }

}
