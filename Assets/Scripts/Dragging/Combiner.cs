using UnityEngine;
using System.Collections.Generic;

public class Combiner : MonoBehaviour
{
    [SerializeField] List<Recipe> recipes = new List<Recipe>();
    List<FoodType> stored = new();


    public void AddFood(FoodType food)
    {
        stored.Add(food);
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
            if (i == recipes.Count)
            {
                FoodReferenceTable.Instance.Spawn(recipe.result, transform.position);
                return;
            }
        }

        if (stored.Count >= maxRecipe)
        {
            foreach (FoodType food in stored)
            {
                FoodReferenceTable.Instance.Spawn(food, transform.position);
            }
            stored.Clear();
        }
    }
}
