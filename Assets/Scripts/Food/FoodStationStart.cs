using UnityEngine;

public class FoodStationStart : RecipeCheck
{
    [SerializeField] GameObject workAreaPrefab;
    FoodStationWorkArea workArea;

    private void Awake()
    {

        workArea = Instantiate(workAreaPrefab).GetComponentInChildren<FoodStationWorkArea>();
        workArea.stationStart = this;
    }

    public override void RecipeMatched(FoodType foodType)
    {
        workArea.Activate(foodType);
    }
}
