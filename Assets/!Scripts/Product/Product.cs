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
        [field: SerializeField, SerializeReference, SubclassSelector]public List<IModification> modifications { get; private set; } = new();

        public MeshRenderer artRenderer {  get; private set; }
        
        private void Start()
        {
            artRenderer = GetComponentInChildren<MeshRenderer>();
            GameManager.onStop += OnStop;

            Tick.OnLateTick += () =>
            {
                if(modifications.Count <= 0) return;
                foreach (var mod in modifications)
                {
                    mod.Apply(this);
                }
            };
        }
        private void OnDestroy()
        {
            GameManager.onStop -= OnStop;
        }

        private void OnStop()
        {
            Destroy(gameObject);
        }
        
        public void Modify(IModification mod)
        {
            modifications.Add(mod);
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

