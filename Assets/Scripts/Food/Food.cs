using System.Collections.Generic;
using UnityEngine;

public class Food : Draggable
{
    public FoodType type;

    HashSet<FoodInteractable> interactbales = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FoodInteractable>(out FoodInteractable combiner))
        {
            if (!interactbales.Contains(combiner)) { interactbales.Add(combiner); }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FoodInteractable>(out FoodInteractable combiner))
        {
            if (interactbales.Contains(combiner)) { interactbales.Remove(combiner); }
        }
    }

    public override void OnRelease()
    {
        base.OnRelease();
        foreach (FoodInteractable combiner in interactbales)
        {
            if (combiner == null) { continue; }
            combiner.AddFood(type);
            Destroy(gameObject);
            return;
        }
    }
}
