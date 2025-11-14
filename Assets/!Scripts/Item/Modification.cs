using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using UnityEngine;

public class Modification
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

    public virtual void Apply(Item item)
    {

    }
    public virtual bool Compare(Modification toCompareWith)
    {
        bool sameName = Name == toCompareWith.Name;
        bool sameDescription = Description == toCompareWith.Description;

        return sameName && sameDescription;
    }
    public class Assemble : Modification
    {
        public Assemble(string Name) : base("Assembler: Craft", Name)
        {
        }

        public override void Apply(Item item)
        {
            //its okay this doesnt need to do anything :)
        }
    }

    public class Color : Modification
    {
        static readonly Dictionary<UnityEngine.Color, string> colorMap = new Dictionary<UnityEngine.Color, string>()
        {
            {UnityEngine.Color.red, "Red"},
            {UnityEngine.Color.blue, "Blue"},
            {UnityEngine.Color.green, "Green"},
        };

        public UnityEngine.Color color;

        public Color(UnityEngine.Color color) : base("Painter: Color", colorMap[color])
        {
            this.color = color;
        }

        public override void Apply(Item item)
        {
            if(!item.changedColor)
            {
                item.artRenderer.material.SetColor("_Colour", new UnityEngine.Color(0, 0, 0, 1));
                item.changedColor = true;
            }

            item.artRenderer.material.SetColor("_Colour", item.artRenderer.material.GetColor("_Colour") + color);
        }

        public override bool Compare(Modification toCompareWith)
        {
            return ((Color)toCompareWith).color == color;
        }
    }
}