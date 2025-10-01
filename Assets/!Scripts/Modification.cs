using System;

public abstract class Modification
{
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

    public class Heated : Modification
    {
        public int temperature;

        public Heated(Item toModify, int temperature)
        {
            this.temperature = temperature;
        }

        public override void Apply(Item item)
        {
            throw new System.NotImplementedException();
        }

        public override bool Compare(Modification toCompareWith) 
        {
            return false;
        }
    }

    public class Addition : Modification
    {
        public Materials materials;
        public Addition(Materials materials)
        {
            this.materials = materials;
        }

        public override void Apply(Item item)
        {
            throw new System.NotImplementedException();
        }

        public override bool Compare(Modification toCompareWith)
        {
            return false;
        }
    }

    public class Color : Modification
    {
        public UnityEngine.Color color;

        public Color(UnityEngine.Color color)
        {
            this.color = color;
        }

        public override void Apply(Item item)
        {
            item.artRenderer.material.SetColor("_Color", item.artRenderer.material.GetColor("_Color") + color);
        }

        public override bool Compare(Modification toCompareWith)
        {
            return ((Color)toCompareWith).color == color;
        }
    }
}