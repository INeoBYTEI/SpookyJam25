using System.Collections.Generic;
using UnityEngine;

public class FoodInteractable : MonoBehaviour
{
    protected List<FoodType> stored = new();

    public void AddFood(FoodType food, Vector3 pos)
    {
        stored.Add(food);
        OnFoodAdded(food, pos);
    }

    public virtual void OnFoodAdded(FoodType food, Vector3 pos) { }
}
