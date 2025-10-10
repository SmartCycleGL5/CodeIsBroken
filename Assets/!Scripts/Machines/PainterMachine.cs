using System;
using Unity.VisualScripting;
using UnityEngine;

public class PainterMachine : Machine
{
    [SerializeField] public Item item;
    private UserErrorLogger errorLogger;
    Renderer toColor { get { return item.artRenderer; } }
    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(PainterMachine));

        base.Initialize(initialClassName);
    }

    private void OnEnable()
    {
        errorLogger = GetComponent<UserErrorLogger>();
        
    }

    public void Paint(string color)
    {
        Debug.Log("Set color to: " + color);
        if (item == null) return;

        switch (color)
        {
            case "Red":
                item.definition.Modify(new Modification.Color(new Color(1, 0, 0)));
                return;
            case "Blue":
                item.definition.Modify(new Modification.Color(new Color(0, 0, 1)));
                return;
            case "Green":
                item.definition.Modify(new Modification.Color(new Color(0, 1, 0)));
                return;
            default:
                errorLogger.DisplayError("Not a valid color!");
                return;

        }
    }

    private void OnDestroy()
    {
        if (item == null) return;
        Destroy(item);
    }

}
