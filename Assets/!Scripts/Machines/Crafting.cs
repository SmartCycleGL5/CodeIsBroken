using System.Collections.Generic;
using System.Linq;
using CodeIsBroken.Product;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public static Crafting instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public Item CraftItem(List<Item> items, List<CraftingRecipie> craftingRecipies)
    {
        if(items.Count == 0) return null;
        List<ProductDefinition> materials = new();
        List<ProductDefinition> recipeItems = new();
        materials.AddRange(items.Select(i => i.definition));
        
        //Loops over all recipes
        foreach (var recipe in craftingRecipies)
        {
            // Sort lists to compare them:
            materials = materials.OrderBy(x => x.name).ToList();
            recipeItems.AddRange(recipe.materials.Select(material => material.definition));
            recipe.materials = recipe.materials.OrderBy(x => x.name).ToList();

            for (int i = 0; i < recipeItems.Count; i++)
            {
                if (!recipeItems[i].Equals(materials[i], false))
                {
                    Debug.Log("MAterial not equal");
                    break;
                }

                if (recipeItems.Count >= i)
                {
                    Debug.Log("Valid recipe");

                    return recipe.itemToSpawn;
                }
                
            }
            
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
