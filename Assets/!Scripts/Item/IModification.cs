using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using UnityEngine;

namespace CodeIsBroken.Product.Modifications
{
    public interface IModification : IEquatable<IModification>
    {
        public string Name { get; }
        public string Description { get; }
        
        public void Apply(Item item);
        public bool Equals(IModification other);
        
        public static IModification RandomModification()
        {
            int rng = UnityEngine.Random.Range(0, 3);

            switch (rng)
            {
                case 0:
                    {
                        return Color.New(new UnityEngine.Color(1, 0, 0));
                    }
                case 1:
                    {
                        return Color.New(new UnityEngine.Color(0, 1, 0));
                    }
                case 2:
                    {
                        return Color.New(new UnityEngine.Color(0, 0, 1));
                    }
            }
    
            return default;
        }
    }
    
    [Serializable]
    public class Color : IModification
    {
        public string Name => "Color:";
        public string Description => $"Red: {color.r}, Green: {color.g}, Blue: {color.b}";
        [field: SerializeField] public UnityEngine.Color color  { get; private set; }
        
        public static Color New(UnityEngine.Color color)
        {
            Color toReturn = new Color();
            toReturn.color = color;
            return toReturn;
        }

        public void Apply(Item item)
        {
            if(!item.changedColor)
            {
                item.artRenderer.material.SetColor("_Colour", new UnityEngine.Color(0, 0, 0, 1));
                item.changedColor = true;
            }
    
            item.artRenderer.material.SetColor("_Colour", item.artRenderer.material.GetColor("_Colour") + color);
        }

        public bool Equals(IModification other)
        {
            if (other is null) return false;
            if (other is not Color) return false;
            if (Name != other.Name) return false;
            if (Description != other.Description) return false;
            if (color != ((Color)other).color) return false;
            
            return true;
        }
    }

    [Serializable]
    public class Cut : IModification
    {
        public string Name => "Cut";
        public string Description => "pls cut";
        public void Apply(Item item)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IModification other)
        {
            if (other is null) { Debug.Log("other is null"); return false; }
            if (other is not Cut) { Debug.Log("other is not cut"); return false; }
            if (Name != other.Name) return false;
            if (Description != other.Description) return false;
            
            return true;
        }
    }

    [Serializable]
    public class Assembled : IModification
    {
        public string Name => "Assembled";
        public string Description => "nice chair lol";
        public void Apply(Item item)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IModification other)
        {
            if (other is null) { Debug.Log("other is null"); return false; }
            if (other is not Assembled) { Debug.Log("other is not assembled"); return false; }
            if (Name != other.Name) return false;
            if (Description != other.Description) return false;
            
            return true;
        }
    }
}
