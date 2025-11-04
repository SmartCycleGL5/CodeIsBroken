using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class CraneMachine : Machine, IItemContainer
{
    [Space(10), SerializeField] Transform piviot;
    [SerializeField] Transform grabLocation;
    [SerializeField] Transform holdLocation;

    Tweener rotationTween;
    private bool pickUp;
    private bool drop;
    [DontIntegrate] public Item item { get; set; }


    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(CraneMachine));

        base.Initialize(initialClassName);
    }

    public void Rotate(int degrees)
    {
        Metrics.instance.UseElectricity((int)degrees/90);

        if (rotationTween != null) return;
        rotationTween = piviot.DORotate(new Vector3(0, piviot.rotation.eulerAngles.y+degrees, 0), 0.5f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            rotationTween = null;
            if(pickUp) GrabLoseItem();
            pickUp = false;
        });
    }

    public void PickUp()
    {
        if (rotationTween != null)
        {
            pickUp = true;
        }
        else
        {
            GrabLoseItem();
        }
    }
    
    public void GrabLoseItem()
    {
        //Metrics.instance.UseElectricity(1);
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
