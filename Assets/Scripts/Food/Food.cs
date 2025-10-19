using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Food : Draggable
{
    public FoodType type;
    public AudioSource audioSource;

    HashSet<FoodInteractable> interactables = new();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FoodInteractable>(out FoodInteractable combiner))
        {
            if (!interactables.Contains(combiner)) { interactables.Add(combiner); }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FoodInteractable>(out FoodInteractable combiner))
        {
            if (interactables.Contains(combiner)) { interactables.Remove(combiner); }
        }
    }

    public override void OnClick()
    {
        base.OnClick();
        PlayCorrectFoodSound(type, audioSource);
    }
    
    public static void PlayCorrectFoodSound(FoodType type, AudioSource audioSource)
    {
        switch (type)
        {
            case FoodType.Nothing:
                break;
            case FoodType.Burger:
                AudioManager.Instance.PlayAudio("GrabBurger", audioSource);
                break;
            case FoodType.Fries:
                AudioManager.Instance.PlayAudio("Crunch", audioSource);
                break;
            case FoodType.SingleFry:
                AudioManager.Instance.PlayAudio("Crunch", audioSource);
                break;
            case FoodType.Soda:
                AudioManager.Instance.PlayAudio("GrabDrink", audioSource);
                break;
        }
    }

    public override void OnRelease()
    {
        base.OnRelease();
        foreach (FoodInteractable combiner in interactables)
        {
            if (combiner == null) { continue; }
            combiner.AddFood(type, transform.position);
            Destroy(gameObject);
            return;
        }
    }
}
