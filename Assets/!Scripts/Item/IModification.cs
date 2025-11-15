using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using UnityEngine;

namespace CodeIsBroken.Product.Modification
{
    public interface IModification
    {
        public string Name {  get; set; }
        public string Description {  get; set; }
        
        public void Apply(Item item);
        
        public static IModification RandomModification()
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
    
            return default;
        }
    }
    public interface IModification<T> : IModification, IEquatable<T> where T : IModification<T>
    {
    }
    
    public class Color : IModification<Color>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public UnityEngine.Color color  { get; private set; }
    
        public Color(UnityEngine.Color color)
        {
            Name = $"Colors:";
            Description = $"Red: {color.r}, Green: {color.g}, Blue: {color.b}";
            this.color = color;
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

        public bool Equals(Color other)
        {
            if (other is null) return false;
            if (Name != other.Name) return false;
            if (Description != other.Description) return false;
            if (color != other.color) return false;
            
            return true;
        }
    }
}
