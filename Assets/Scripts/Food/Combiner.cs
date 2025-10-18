using UnityEngine;

public class Combiner : RecipeCheck
{
    public override void RecipeMatched(FoodType food)
    {
        FoodReferenceTable.Instance.SpawnFood(food, transform.position);
    }
}
