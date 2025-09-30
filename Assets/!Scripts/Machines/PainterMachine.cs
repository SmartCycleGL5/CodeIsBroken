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
        Debug.Log("Set color to: " + color);
        if (item == null) return;

        switch (color)
        {
            case "red":
                item.Modify(new ModColor(new Color(1, 0, 0)));
                toColor.material.SetColor("_Color", new Color(1, 0, 0));
                return;
            case "blue":
                item.Modify(new ModColor(new Color(0, 0, 1)));
                toColor.material.SetColor("_Color", new Color(0, 0, 1));
                return;
            case "green":
                item.Modify(new ModColor(new Color(0, 1, 0)));
                toColor.material.SetColor("_Color", new Color(0, 1, 0));
                return;
            default:
                return;

        }
    }

    private void OnDestroy()
    {
        if (item == null) return;
        Destroy(item);
    }

}
