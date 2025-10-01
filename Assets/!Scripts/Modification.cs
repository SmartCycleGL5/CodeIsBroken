public abstract class Modification
{
    protected Item toModify;

    public Modification(Item toModify)
    {
        this.toModify = toModify;
        toModify.Modify(this);
    }

    public static Modification RandomModification(Item toMod)
    {
        int rng = UnityEngine.Random.Range(0, 2);

        switch (rng)
        {
            case 0:
                {
                    return new Color(toMod, new UnityEngine.Color(1, 0, 0));
                }
            case 1:
                {
                    return new Color(toMod, new UnityEngine.Color(0, 1, 0));
                }
            case 2:
                {
                    return new Color(toMod, new UnityEngine.Color(0, 0, 1));
                }
        }

        return null;
    }

    public abstract bool Compare(Modification toCompareWith);

    public class Heated : Modification
    {
        public int temperature;

        public Heated(Item toModify, int temperature) : base(toModify)
        {
            this.temperature = temperature;
        }

        public override bool Compare(Modification toCompareWith) 
        {
            return false;
        }
    }

    public class Addition : Modification
    {
        public BaseMaterial.Materials materials;
        public Addition(Item toModify, BaseMaterial.Materials materials) : base(toModify)
        {
            this.materials = materials;
        }

        public override bool Compare(Modification toCompareWith)
        {
            return false;
        }
    }

    public class Color : Modification
    {
        public UnityEngine.Color color;

        public Color(Item toModify, UnityEngine.Color color) : base(toModify)
        {
            this.color = color;
            toModify.artRenderer.material.SetColor("_Color", toModify.artRenderer.material.GetColor("_Color") + color);
        }

        public override bool Compare(Modification toCompareWith)
        {
            return ((Color)toCompareWith).color == color;
        }
    }
}