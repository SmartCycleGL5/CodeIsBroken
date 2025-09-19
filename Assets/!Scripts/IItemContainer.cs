using UnityEngine;

public interface IItemContainer
{
    public Item item {  get; set; }

    public bool SetItem(Item item);
    public void RemoveItem();
    //Steven was here
}
