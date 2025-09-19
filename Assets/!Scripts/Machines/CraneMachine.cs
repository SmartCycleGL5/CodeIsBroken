using System.Threading.Tasks;
using UnityEngine;

public class CraneMachine : Machine, IItemContainer
{
    [Space(10), SerializeField] Transform piviot;
    [SerializeField] Transform grabLocation;

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

    public void GrabItem()
    {
        GameObject cell = GridBuilder.instance.LookUpCell(transform.position + transform.forward);

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

        item = conveyor.item;
    }

    public void RemoveItem(Item item)
    {
        throw new System.NotImplementedException();
    }
}
