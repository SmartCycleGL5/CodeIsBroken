using System;

[Serializable]
public class Modification
{
    public string name;

    public Modification(string name)
    {
        this.name = name;
    }

    public class Heated : Modification
    {
        public int temperature;

        public Heated(string name, int temperature) : base(name)
        {
            this.temperature = temperature;
        }
    }

    public class Addition : Modification
    {
        public BaseMaterial.Materials materials;
        public Addition(string name, BaseMaterial.Materials materials) : base(name)
        {
            this.materials = materials;
        }
    }

    public class Color : Modification
    { 
        public UnityEngine.Color color;
        public Color(string name, UnityEngine.Color color) : base(name)
        {
            this.color = color;
        }
    }
}