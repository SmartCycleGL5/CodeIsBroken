using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using UnityEngine;

namespace CodeIsBroken.ProductSystem.Modifications
{
    public interface IModification : IEquatable<IModification>
    {
        public string Name { get; }
        public string Description { get; }
        
        public bool Equals(IModification other);
        public void Apply(Product item);
    }
/*
    public interface IAdditionalModification : IModification
    {
        public void Apply(Product item);

        public static IAdditionalModification RandomModification()
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
        public static IAdditionalModification[] GetRandomModifications(int amount)
        {
            List<IAdditionalModification> mods = new List<IAdditionalModification>();
            for (int i = 0; i < amount; i++)
            {
                IAdditionalModification newMod = RandomModification();

                if (mods.Contains(newMod))
                {
                    Debug.Log("already has mod");
                    continue;
                }

                Debug.Log(newMod);
                mods.Add(newMod);
            }

            return mods.ToArray();
        }
    }*/

    [Serializable]
    public class Color : IModification
    {
        public string Name => $"Paint: {Description}";

        public string Description
        {
            get
            {
                if(color.r > 0) 
                    return "Red";
                if (color.g > 0)
                    return "Green";
                if (color.b > 0)
                    return "Blue";

                return "Fail";
            }
        }

        public bool Applied { get; set; }
        [field: SerializeField] public UnityEngine.Color color  { get; private set; } = new UnityEngine.Color(0, 0, 0, 1);
        
        public static Color New(UnityEngine.Color color)
        {
            Color toReturn = new Color();
            toReturn.color = color;
            return toReturn;
        }
        
        public bool Equals(IModification other)
        {
            if (other is null) return false;
            if (other is not Color) return false;
            if (color != ((Color)other).color) return false;
            
            return true;
        }

        public void Apply(Product item)
        {
            if(Applied) return;
            if(!item.artRenderer.material.GetColor("_Colour").Equals(UnityEngine.Color.white))
            {
                item.artRenderer.material.SetColor("_Colour", new UnityEngine.Color(0, 0, 0, 1));
            }
    
            item.artRenderer.material.SetColor("_Colour", item.artRenderer.material.GetColor("_Colour") + color);
            Applied = true;
        }
        
        public static Color Random()
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
/*
    [Serializable]
    public class Cut : IModification
    {
        public string Name => "Cut";
        public string Description => "";

        public bool Equals(IModification other)
        {
            if (other is null) { Debug.Log("other is null"); return false; }
            if (other is not Cut) { Debug.Log("other is not cut"); return false; }
            
            return true;
        }
    }

    [Serializable]
    public class Assembled : IModification
    {
        public string Name => "Assembled";
        public string Description => "";

        public bool Equals(IModification other)
        {
            if (other is null) { Debug.Log("other is null"); return false; }
            if (other is not Assembled) { Debug.Log("other is not assembled"); return false; }
            
            return true;
        }
    }
    [Serializable]
    public class Melted : IModification
    {
        public string Name => "Melted";

        public string Description => "";

        public bool Equals(IModification other)
        {
            if (other is null) { Debug.Log("other is null"); return false; }
            if (other is not Melted) { Debug.Log("other is not melted"); return false; }

            return true;
        }
    }*/
}
