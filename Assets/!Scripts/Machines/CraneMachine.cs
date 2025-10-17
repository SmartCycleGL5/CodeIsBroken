using System.Threading.Tasks;
using UnityEngine;

public class CraneMachine : Machine, IItemContainer
{
    [Space(10), SerializeField] Transform piviot;
    [SerializeField] Transform grabLocation;
    [SerializeField] Transform holdLocation;

    [DontIntegrate] public Item item { get; set; }

    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(CraneMachine));

        base.Initialize(initialClassName);
    }

    public async void Rotate(int degrees)
    {
        float timer = 0;
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
            if (conveyor.item == null)
            {
                Debug.Log("[Crane] No Item on conveyor");
                return;
            }

            if (SetItem(conveyor.item))
            {
                Debug.Log("[Crane] Grab");
                conveyor.RemoveItem();
            }
        }
        else if (conveyor.SetItem(item))
        {
            RemoveItem();
        }

    }
    [DontIntegrate]
    public bool SetItem(Item item)
    {

        if (this.item != null) return false;

        this.item = item;
        this.item.transform.parent = holdLocation;
        this.item.transform.position = holdLocation.position;
        return true;
    }
    [DontIntegrate]
    public bool RemoveItem(out Item removedItem)
    {
        removedItem = null;
        if (item == null) return false;
        removedItem = item;
        item.transform.parent = null;
        item = null;
        return true;
    }
    [DontIntegrate]
    public bool RemoveItem()
    {
        return RemoveItem(out Item item);
    }

}
