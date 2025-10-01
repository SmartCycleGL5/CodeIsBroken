public abstract class Modification
{

    public static Modification RandomModification()
    {
        int rng = UnityEngine.Random.Range(0, 2);

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

    public abstract bool Compare(Modification toCompareWith);

    public class Heated : Modification
    {
        public int temperature;

        public Heated(int temperature)
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
        public Addition(BaseMaterial.Materials materials)
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
        public Color(UnityEngine.Color color)
        {
            this.color = color;
        }

        public override bool Compare(Modification toCompareWith)
        {
            return ((Color)toCompareWith).color == color;
        }
    }
}