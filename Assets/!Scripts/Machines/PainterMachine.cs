using UnityEngine;

public class PainterMachine : Machine
{
    [SerializeField] Item item;
    [SerializeField] Material redMaterial;
    [SerializeField] Material blueMaterial;
    [SerializeField] ConveyorItemReciever reciever;
    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(PainterMachine));

        base.Initialize(initialClassName);
    }

    

    public void Paint(string color)
    {
        Renderer itemColor = item.gameObject.GetComponent<Renderer>();
        switch (color)
        {
            case "red":
                itemColor.material = redMaterial;
                return;
            case "blue":
                itemColor.material = blueMaterial;
                return;
            default:
                return;

        }
    }

}
