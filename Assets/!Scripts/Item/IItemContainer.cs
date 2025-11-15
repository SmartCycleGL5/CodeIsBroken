using UnityEngine;

namespace CodeIsBroken.Product
{
    public interface IItemContainer
    {
        public Item item {  get; set; }
    
        public bool SetItem(Item item);
        public bool RemoveItem(out Item item);
        public bool RemoveItem();
    }
}

