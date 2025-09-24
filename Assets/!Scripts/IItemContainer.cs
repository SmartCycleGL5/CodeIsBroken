using UnityEngine;

public interface IItemContainer
{
    public Item item {  get; set; }
    public bool SetItem(Item item);
    public bool RemoveItem(out Item item);
    public bool RemoveItem();
    //Steven was here
}
