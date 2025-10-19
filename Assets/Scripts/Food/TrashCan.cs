using UnityEngine;

public class TrashCan : FoodInteractable
{
    public override void OnFoodAdded(FoodType food, Vector3 pos)
    {
        AudioManager.Instance.PlayAudio("ThrowAway", null, false, 1, true, 0, true);
    }
}
