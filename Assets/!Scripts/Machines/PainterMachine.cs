using UnityEngine;

public class PainterMachine : Machine
{
    [SerializeField] public Item item;
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
                item.Modify(new Modification.Color("ColorVariant", new Color(1, 0, 0)));
                itemColor.material.color = new Color(1, 0, 0);
                return;
            case "blue":
                item.Modify(new Modification.Color("ColorVariant", new Color(0, 1, 0)));
                itemColor.material.color = new Color(0, 1, 0);
                return;
            default:
                return;

        }
    }

}
