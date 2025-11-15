using System;
using System.Collections.Generic;
using CodeIsBroken.Product.Modification;
using UnityEngine;

namespace CodeIsBroken.Product
{
    [Flags]
    public enum Materials
    {
        None = 0,
        Wood = 1 << 1,
        Iron = 1 << 2,
        Stone = 1 << 3,
    }
    [Serializable]
    public class ProductDefinition : IEquatable<ProductDefinition>
    {
        #region Quick access to "base materials" 

        public static ProductDefinition Wood => new (Materials.Wood);
        public static ProductDefinition Iron  => new (Materials.Iron);
        public static ProductDefinition Stone => new (Materials.Stone);

        #endregion
        
        
        public Materials materials;

        public List<IModification> mods { get; set; } = new();
        
        public Action<IModification> modified;
    
        public ProductDefinition(Materials materials, List<IModification> modifications = null)
        {
            this.materials = materials;

            if (modifications != null)
            {
                mods = modifications;   
            }
            else
            {
                mods = new List<IModification>();
            }
        }
    
        public void Modify(IModification modification)
        {
            if (mods.Contains(modification))
            {
                Debug.Log("Already has mod");
                return;
            }
    
            mods.Add(modification);
            modified?.Invoke(modification);
        }

        public bool Equals(ProductDefinition other)
        {
            if (other is null){ return false;}
            if (!(mods == null && other.mods == null)) { Debug.Log($"{mods} or {other.mods} is null"); return false;}
            if (materials != other.materials) { Debug.Log($"{materials} != {other.materials }"); return false;}

            if (mods == null || other.mods == null) return true;
            for (int i = 0; i < mods.Count; i++)
            {
                if(mods[i] != other.mods[i]) { Debug.Log($"{mods[i]} != {other.mods[i] }"); return false;}
            }
            
            return true;
        }
    }
}
