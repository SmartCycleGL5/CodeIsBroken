using System;

public interface Modification
{

    public static Modification RandomModification()
    {
        int rng = UnityEngine.Random.Range(0, 2);

        switch (rng)
        {
            case 0:
                {
                    return new ModColor(new UnityEngine.Color(1, 0, 0));
                }
            case 1:
                {
                    return new ModColor(new UnityEngine.Color(0, 1, 0));
                }
            case 2:
                {
                    return new ModColor(new UnityEngine.Color(0, 0, 1));
                }
        }

        return null;
    }
}

    public struct Heated : Modification
    {
        public int temperature;

        public Heated(int temperature) 
        {
            this.temperature = temperature;
        }
    }

    public struct Addition : Modification
    {
        public BaseMaterial.Materials materials;
        public Addition(BaseMaterial.Materials materials)
        {
            this.materials = materials;
        }
    }

    public struct ModColor : Modification
    { 
        public UnityEngine.Color color;
        public ModColor(UnityEngine.Color color)
        {
            this.color = color;
        }
    }