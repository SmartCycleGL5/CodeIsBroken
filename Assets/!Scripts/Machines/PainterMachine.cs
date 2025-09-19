using UnityEngine;

public class PainterMachine : Machine
{
    [SerializeField] public Item item;
    [SerializeField] Material redMaterial;
    [SerializeField] Material blueMaterial;
    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(PainterMachine));

        base.Initialize(initialClassName);
    }


    public void Paint(string color)
    {
        if(item == null) return;
        Renderer itemColor = item.gameObject.GetComponentInChildren<Renderer>();
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
