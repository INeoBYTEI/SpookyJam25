using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="New Recipe", menuName = "Food Recipe")]
public class Recipe : ScriptableObject
{
    public List<FoodType> required = new();
    public FoodType result;
}
