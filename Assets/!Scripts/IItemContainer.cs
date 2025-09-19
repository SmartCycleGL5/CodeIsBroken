using UnityEngine;

public interface IItemContainer
{
    public Item item {  get; set; }
    [IgnoreMethod]
    public bool SetItem(Item item);
    [IgnoreMethod]
    public void RemoveItem();
    //Steven was here
}
