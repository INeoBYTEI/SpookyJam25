using UnityEngine;
using UnityEngine.UIElements;

public class DragSpawner : Clickable
{
    [SerializeField] FoodType foodType;

    public override void OnClick()
    {
        Food food = FoodReferenceTable.Instance.Spawn(foodType, transform.position);
        if (food != null) { Clicker.Instance.AddClicked(food); }
    }
}
