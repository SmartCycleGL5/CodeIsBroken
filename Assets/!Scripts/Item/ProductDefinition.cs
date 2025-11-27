using CodeIsBroken.Product.Modifications;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    public class ProductDefinition
    {
        #region Quick access to "base materials" 

        public static ProductDefinition Wood => new(BaseMaterials.Wood);
        public static ProductDefinition Iron => new(BaseMaterials.Iron);
        public static ProductDefinition Stone => new(BaseMaterials.Stone);

        #endregion

        public string name;
        public Sprite icon;
        [FormerlySerializedAs("materials")] public BaseMaterials baseMaterials;

        [field: SerializeReference, SubclassSelector]
        public List<IModification> baseMods { get; private set; } = new();
        public List<IModification> additionalMods { get; private set; } = new();

        public Action<IAdditionalModification> modified;

        public Guid id;

        public ProductDefinition(BaseMaterials baseMaterials, List<IModification> baseMods = null)
        {
            this.baseMaterials = baseMaterials;
            id = Guid.NewGuid();

            this.baseMods = new List<IModification>();

            if (baseMods != null)
                this.baseMods = baseMods;
        }

        public void Modify(IModification modification)
        {
            if (additionalMods.Contains(modification))
            {
                Debug.Log("Already has mod");
                return;
            }

            additionalMods.Add(modification);

            if (modification is IAdditionalModification)
                modified?.Invoke((IAdditionalModification)modification);
        }

        public bool Equals(ProductDefinition other, bool includeAdditionalMods = true)
        {
            if (baseMods == null || other.baseMods == null) { return false; }
            if (baseMaterials != other.baseMaterials) { Debug.Log($"{baseMaterials} != {other.baseMaterials}"); return false; }
            if (baseMods.Count != other.baseMods.Count) return false;

            for (int i = 0; i < baseMods.Count; i++)
            {
                if (!baseMods[i].Equals(other.baseMods[i])) { Debug.Log($"{baseMods[i]} != {other.baseMods[i]}"); return false; }
            }
            if (includeAdditionalMods)
            {
                if (additionalMods.Count != other.additionalMods.Count) return false;

                for (int i = 0; i < additionalMods.Count; i++)
                {
                    if (!additionalMods[i].Equals(other.additionalMods[i])) { Debug.Log($"{additionalMods[i]} != {other.additionalMods[i]}"); return false; }
                }
            }


            return true;
        }

        public ProductDefinition Clone()
        {
            ProductDefinition clone = new ProductDefinition(baseMaterials);

            foreach (var mod in baseMods)
            {
                clone.baseMods.Add(mod);
            }
            foreach (var mod in additionalMods)
            {
                clone.additionalMods.Add(mod);
            }

            clone.icon = icon;
            clone.name = name;

            return clone;
        }
    }
}
