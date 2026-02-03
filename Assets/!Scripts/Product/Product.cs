using System;
using System.Collections.Generic;
using CodeIsBroken.ProductSystem.Modifications;
using UnityEngine;

namespace CodeIsBroken.ProductSystem
{
    public class Product : MonoBehaviour
    {
        public Sprite icon;

        [Min(1)]
        public int lvlUnlock = 1;

        public Products pruductType;
        [field: SerializeField]public List<IModification> modifications { get; private set; } = new();

        public MeshRenderer artRenderer {  get; private set; }
        
        private void Start()
        {
            artRenderer = GetComponentInChildren<MeshRenderer>();
            GameManager.onStop += OnStop;
        }
        private void OnDestroy()
        {
            GameManager.onStop -= OnStop;
        }

        private void OnStop()
        {
            Destroy(gameObject);
        }

        void ApplyModifications(IModification mod)
        {
            mod.Apply(this);
        }

        public void Modify(IModification mod)
        {
            modifications.Add(mod);
            mod.Apply(this);
        }

        public bool Equals(Product other)
        {
            if (pruductType != other.pruductType)
            {
                return false;
            }
            
            return true;
        }
    }
}

