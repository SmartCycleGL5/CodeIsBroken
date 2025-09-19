using UnityEngine;

public interface IItemContainer
{
    public Item item {  get; set; }

    public void RemoveItem(Item item);
}
