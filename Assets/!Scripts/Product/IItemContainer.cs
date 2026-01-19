using UnityEngine;

namespace CodeIsBroken.ProductSystem
{
    public interface IItemContainer
    {
        public Product item {  get; set; }
    
        public bool SetItem(Product item);
        public bool RemoveItem(out Product item);
        public bool RemoveItem();
    }
}

