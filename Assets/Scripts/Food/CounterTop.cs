using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterTop : FoodInteractable
{
    [SerializeField] Transform IconsHolder;
    [SerializeField] GameObject iconPrefab;
    [SerializeField] float forceMultiplier;
    [SerializeField] float torqueMultiplier;
    Customer currentCustomer;

    List<FoodOrder> order = new();

    private void OnEnable()
    {
        CustomerHandler.OnCustomerSpawned += SetCustomer;
    }

    public override void OnFoodAdded(FoodType food, Vector3 pos)
    {
        if (currentCustomer == null)
        {
            ThrowFood(FoodReferenceTable.Instance.SpawnFood(food, pos));
            return;
        }

        int index = 0;
        bool hit = false;
        for (int i = 0; i < order.Count; i++)
        {
            if (order[i].type == food)
            {
                hit = true;
                index = i;
                break;
            }
        }

        if (hit)
        {
            order.RemoveAt(index);
            if (order.Count == 0)
            {
                if (currentCustomer.currentDifficulty == Customer.Difficulty.KAREN) //Karen custom behavior
                {
                    StartCoroutine(currentCustomer.KarenReorder());
                    InsanityManager.Instance.ModifyInsanity(InsanityManager.Instance.karenOrderCompleteInsanity);
                    return;
                }
                CustomerHandler.Instance.customersServed++;
                currentCustomer.StartCoroutine(currentCustomer.Leave());
                for (int i = 0; i < IconsHolder.childCount; i++)
                {
                    Destroy(IconsHolder.GetChild(i).gameObject);
                }
            }
            else
            {
                Image image = Instantiate(iconPrefab, IconsHolder).GetComponent<Image>();
                image.sprite = FoodReferenceTable.Instance.GetSprite(food);
                return;
            }
        }
        else
        {
            ThrowFood(FoodReferenceTable.Instance.SpawnFood(food, pos));
        }
    }

    private void ThrowFood(Draggable draggable)
    {
        draggable.rb.AddForce(Random.insideUnitCircle * forceMultiplier);
        draggable.rb.AddTorque(Random.Range(-1, 1) * torqueMultiplier);
    }

    public void SetCustomer(Customer customer)
    {
        currentCustomer = customer;
        order = customer.orderedMeals;
    }
}
