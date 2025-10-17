using UnityEngine;
using System.Collections.Generic;

public class Recipe : ScriptableObject
{
    public List<FoodType> required = new();
    public FoodType result;
}
