using System.Collections.Generic;
using UnityEngine;

public class RecipeHolder : MonoBehaviour
{
    [SerializeField] private List<CraftingRecipie> recipes = new();

    public List<CraftingRecipie> GetRecipes()
    {
        return recipes;
    }
}
