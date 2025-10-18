using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Food : Draggable
{
    public FoodType type;
    [SerializeField] AudioSource audioSource;

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

    public override void OnClick()
    {
        base.OnClick();
        switch (type) //TODO: Play Correct Sounds
        {
            case FoodType.Nothing:
                break;
            case FoodType.Burger:
                AudioManager.Instance.PlayAudio("GrabBurger", audioSource);
                break;
            case FoodType.Tentacle:
                AudioManager.Instance.PlayAudio("Crunch", audioSource);
                break;
            case FoodType.Bread:
                AudioManager.Instance.PlayAudio("GrabBurger", audioSource);
                break;
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
