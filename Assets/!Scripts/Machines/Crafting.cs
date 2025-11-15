using System.Collections.Generic;
using System.Linq;
using CodeIsBroken.Item;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public static Crafting instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public Item CraftItem(List<Item> items, List<CraftingRecipie>  craftingRecipies)
    {
        List<Materials> materials = new();
        materials.AddRange(items.Select(i => i.definition.materials));
        
        //Loops over all recipes
        foreach (var recipe in craftingRecipies)
        {
            // Sort lists to compare them:
            materials = materials.OrderBy(x => x).ToList();
            recipe.materials = recipe.materials.OrderBy(x => x).ToList();
            
            if (materials.SequenceEqual(recipe.materials))
            {
                Debug.Log("Valid recipe");

                return recipe.itemToSpawn;
            }
            else
            {
                return null;
            }
        }

        return null;
    }
}
