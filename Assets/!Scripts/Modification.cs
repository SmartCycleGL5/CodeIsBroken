using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using UnityEngine;

public abstract class Modification
{
    public string Name {  get; private set; }
    public string Description {  get; private set; }

    public Modification(string Name, string Description)
    {
        this.Name = Name;
        this.Description = Description;
    }

    public static Modification RandomModification()
    {
        int rng = UnityEngine.Random.Range(0, 3);

        switch (rng)
        {
            case 0:
                {
                    return new Color(new UnityEngine.Color(1, 0, 0));
                }
            case 1:
                {
                    return new Color(new UnityEngine.Color(0, 1, 0));
                }
            case 2:
                {
                    return new Color(new UnityEngine.Color(0, 0, 1));
                }
        }

        return null;
    }

    public abstract void Apply(Item item);
    public abstract bool Compare(Modification toCompareWith);

    public class Color : Modification
    {
        static readonly Dictionary<UnityEngine.Color, string> colorMap = new Dictionary<UnityEngine.Color, string>()
        {
            {UnityEngine.Color.red, "Red"},
            {UnityEngine.Color.blue, "Blue"},
            {UnityEngine.Color.green, "Green"},
        };

        public UnityEngine.Color color;

        public Color(UnityEngine.Color color) : base("Color", colorMap[color])
        {
            this.color = color;
        }

        public override void Apply(Item item)
        {
            if(!item.changedColor)
            {
                item.artRenderer.material.SetColor("_Color", new UnityEngine.Color(0, 0, 0, 1));
                item.changedColor = true;
            }

            item.artRenderer.material.SetColor("_Color", item.artRenderer.material.GetColor("_Color") + color);
        }

        public override bool Compare(Modification toCompareWith)
        {
            return ((Color)toCompareWith).color == color;
            
        }
    }
}