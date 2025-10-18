using System.Collections.Generic;
using UnityEngine;

public class Food : Draggable
{
    public FoodType type;

    HashSet<Combiner> combiners = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Combiner>(out Combiner combiner))
        {
            if (!combiners.Contains(combiner)) { combiners.Add(combiner); }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Combiner>(out Combiner combiner))
        {
            if (combiners.Contains(combiner)) { combiners.Remove(combiner); }
        }
    }

    public override void OnRelease()
    {
        base.OnRelease();
        foreach (Combiner combiner in combiners)
        {
            if (combiner == null) { continue; }
            combiner.AddFood(type);
            Destroy(gameObject);
            return;
        }
    }
}
