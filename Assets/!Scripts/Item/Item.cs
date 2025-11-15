using System;
using System.Collections.Generic;
using CodeIsBroken.Product.Modification;
using UnityEngine;

namespace CodeIsBroken.Product
{
    public class Item : MonoBehaviour
    {
        public ProductDefinition definition = new(Materials.Wood);
        public Sprite icon;
    
        public MeshRenderer artRenderer;
        public static List<Item> items = new List<Item>();
        
        [HideInInspector] public bool changedColor;
    
        private void Start()
        {
            items.Add(this);
    
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

