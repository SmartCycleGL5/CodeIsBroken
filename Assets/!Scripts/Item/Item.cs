using System;
using System.Collections.Generic;
using CodeIsBroken.Product.Modifications;
using UnityEngine;

namespace CodeIsBroken.Product
{
    public class Item : MonoBehaviour
    {
        [Min(1)]
        public int lvlUnlock = 1;
        public ProductDefinition definition = new(BaseMaterials.Wood);
    
        public MeshRenderer artRenderer;
        public static List<Item> items = new List<Item>();
        
        [HideInInspector] public bool changedColor;
    
        private void Start()
        {
            items.Add(this);
            
            if(definition.mods == null) return;
            foreach (var mod in definition.mods)
            {
                mod.Apply(this);
            }
    
            definition.modified += ApplyModifications;
        }
        private void OnDestroy()
        {
            items.Remove(this);
            definition.modified -= ApplyModifications;
        }
    
        void ApplyModifications(IModification mod)
        {
            mod.Apply(this);
        }
    }
}

