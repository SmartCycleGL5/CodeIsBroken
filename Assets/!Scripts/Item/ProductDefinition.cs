using System;
using System.Collections.Generic;
using CodeIsBroken.Product.Modifications;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

namespace CodeIsBroken.Product
{
    [Flags]
    public enum BaseMaterials
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

        public static ProductDefinition Wood => new (BaseMaterials.Wood);
        public static ProductDefinition Iron  => new (BaseMaterials.Iron);
        public static ProductDefinition Stone => new (BaseMaterials.Stone);

        #endregion
        
        public Sprite icon;
        [FormerlySerializedAs("materials")] public BaseMaterials baseMaterials;

        [field: SerializeReference, SubclassSelector]
        public List<IModification> mods { get; set; } = new();
        
        public Action<IAdditionalModification> modified;

        public Guid id;
        
        public ProductDefinition(BaseMaterials baseMaterials, List<IModification> mods = null)
        {
            this.baseMaterials = baseMaterials;
            id = Guid.NewGuid();

            this.mods = new List<IModification>();
            
            if(mods != null)
                this.mods = mods;   
        }
    
        public void Modify(IModification modification)
        {
            if (mods.Contains(modification))
            {
                Debug.Log("Already has mod");
                return;
            }
    
            mods.Add(modification);
            
            if(modification is IAdditionalModification)
                modified?.Invoke((IAdditionalModification)modification);
        }

        public bool Equals(ProductDefinition other)
        {
            if(mods == null || other.mods == null) { return false; }
            if (baseMaterials != other.baseMaterials) { Debug.Log($"{baseMaterials} != {other.baseMaterials }"); return false; }
            if (mods.Count != other.mods.Count) return false;
            
            for (int i = 0; i < mods.Count; i++)
            {
                if(mods[i] == null || other.mods[i] == null) { return false; } 
                if(!mods[i].Equals(other.mods[i])) { Debug.Log($"{mods[i]} != {other.mods[i] }"); return false; }
            }
            
            return true;
        }

        public ProductDefinition Clone()
        {
            ProductDefinition clone = new ProductDefinition(baseMaterials);

            foreach (var mod in mods)
            {
                clone.mods.Add(mod);
            }
            
            clone.icon = icon;
            
            return clone;
        }
    }
}
