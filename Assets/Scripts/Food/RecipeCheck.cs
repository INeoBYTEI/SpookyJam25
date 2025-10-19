using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RecipeCheck : FoodInteractable
{
    [SerializeField] List<Recipe> recipes = new List<Recipe>();

    public override void OnFoodAdded(FoodType food, Vector3 pos)
    {
        CheckRecipes();
    }

    private void CheckRecipes()
    {
        int maxRecipe = 0;
        foreach (Recipe recipe in recipes)
        {
            if (recipe.required.Count > maxRecipe) { maxRecipe = recipe.required.Count; }

            int i = 0;
            foreach (FoodType foodType in recipe.required)
            {
                if (!stored.Contains(foodType)) { break; }
                i++;
            }
            if (i == recipe.required.Count)
            {
                RecipeMatched(recipe.result);
                stored.Clear();
                return;
            }
        }

        if (stored.Count >= maxRecipe)
        {
            foreach (FoodType food in stored)
            {
                FoodReferenceTable.Instance.SpawnFood(food, transform.position);
            }
            stored.Clear();
        }
    }

    public virtual void RecipeMatched(FoodType food) { }
}
