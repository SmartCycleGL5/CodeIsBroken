using System;
using System.Collections.Generic;
using CodeIsBroken.Product.Modifications;
using UnityEngine;

namespace CodeIsBroken.Product
{
    public class Product : MonoBehaviour
    {
        [Min(1)]
        public int lvlUnlock = 1;
        public ProductDefinition definition = new(BaseMaterials.Wood);
    
        public MeshRenderer artRenderer;
        
        [HideInInspector] public bool changedColor;
    
        private void Start()
        {
            if(definition.baseMods == null) return;
            foreach (var mod in definition.baseMods)
            {
                if(mod is IAdditionalModification)
                    ((IAdditionalModification)mod).Apply(this);
            }
    
            definition.modified += ApplyModifications;

            GameManager.onStop += OnStop;
        }
        private void OnDestroy()
        {
            GameManager.onStop -= OnStop;
            definition.modified -= ApplyModifications;
        }

        private void OnStop()
        {
            Destroy(gameObject);
        }

        void ApplyModifications(IAdditionalModification mod)
        {
            mod.Apply(this);
        }
    }
}

