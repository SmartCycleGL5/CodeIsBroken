using System.Collections.Generic;
using System.Linq;
using CodeIsBroken.ProductSystem;
using UnityEngine;


public class Crafting : MonoBehaviour
{
    public static Crafting instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public Product CraftItem(List<Product> items, List<CraftingRecipie> craftingRecipies)
    {
        if(items.Count == 0) return null;
        List<Product> recipeItems = new();
        
        //Loops over all recipes
        foreach (var recipe in craftingRecipies)
        {
            // Sort lists to compare them:
            items = items.OrderBy(x => x.name).ToList();
            recipeItems.AddRange(recipe.materials);
            recipe.materials = recipe.materials.OrderBy(x => x.name).ToList();
            
            if(items.Count != recipeItems.Count) return null;

            for (int i = 0; i < recipeItems.Count; i++)
            {
                if (recipeItems[i] != items[i])
                {
                    Debug.Log("MAterial not equal");
                    return null;
                }
            }
            return recipe.itemToSpawn;
            
            /*if (materials.SequenceEqual(recipeItems))
            {
                
            }
            else
            {
                return null;
            }*/
        }

        return null;
    }
}
