using System.Collections.Generic;
using UnityEngine;

public class FoodInteractable : MonoBehaviour
{
    protected List<FoodType> stored = new();

    public void AddFood(FoodType food)
    {
        stored.Add(food);
        OnFoodAdded(food);
    }

    public virtual void OnFoodAdded(FoodType food) { }
}
