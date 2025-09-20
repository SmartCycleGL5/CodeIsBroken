using UnityEngine;

public class PainterMachine : Machine
{
    [SerializeField] public Item item;
    Renderer toColor { get { return item.artRenderer; } }
    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(PainterMachine));

        base.Initialize(initialClassName);
    }


    public void Paint(string color)
    {
        if(item == null) return;

        switch (color)
        {
            case "red":
                item.Modify(new Modification.Color("ColorVariant", new Color(1, 0, 0)));
                toColor.material.SetColor("_Color", new Color(1, 0, 0));
                return;
            case "blue":
                item.Modify(new Modification.Color("ColorVariant", new Color(0, 0, 1)));
                toColor.material.SetColor("_Color", new Color(0, 0, 1));
                return;
            case "green":
                item.Modify(new Modification.Color("ColorVariant", new Color(0, 1, 0)));
                toColor.material.SetColor("_Color", new Color(0, 1, 0));
                return;
            default:
                return;

        }
    }

}
