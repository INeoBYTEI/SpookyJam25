using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InsanityEvent
{
    [SerializeField] string title;
    public float insanityTriggerValue = 0;
    public UnityEvent triggerd;
    [HideInInspector] public bool hasTriggerd = false;
    public void InvokeEvent()
    {
        if (!hasTriggerd)
        {
            triggerd?.Invoke();
            hasTriggerd = true;
        }
    }
}

public class InsanityManager : MonoBehaviour
{
    public float insanity = 0;
    [SerializeField] InsanityEvent[] insanityEvents;
    Customer currentCustomer;

    [SerializeField] float easyInsanityPerSec = 1;
    [SerializeField] float mediumInsanityPerSec = 2;
    [SerializeField] float hardInsanityPerSec = 3;
    [SerializeField] float insanityRecoverPerSec = 1;

    void Start()
    {
        CustomerHandler.OnCustomerSpawned += SetCurrentCustomer;
    }

    void SetCurrentCustomer(Customer newCustomer)
    {
        currentCustomer = newCustomer;
    }

    void Update()
    {
        foreach (var @event in insanityEvents)
        {
            if (insanity >= @event.insanityTriggerValue)
            {
                @event.InvokeEvent();
            }
            else if (@event.hasTriggerd)
            {
                @event.hasTriggerd = false;
            }
        }

        if (currentCustomer != null)
        {
            if (currentCustomer.currentState == Customer.CustomerState.ORDERING ||
            currentCustomer.currentState == Customer.CustomerState.WAITING)
            {
                AddInsanityBasedOnCustomerDifficulty(currentCustomer);
            }
        }
        else
        {
            ModifyInsanity(-(insanityRecoverPerSec * Time.deltaTime));
        }
    }

    void AddInsanityBasedOnCustomerDifficulty(Customer customer)
    {
        switch (customer.currentDifficulty)
        {
            case Customer.Difficulty.EASY:
                ModifyInsanity(easyInsanityPerSec * Time.deltaTime);
                break;
            case Customer.Difficulty.MEDIUM:
                ModifyInsanity(mediumInsanityPerSec * Time.deltaTime);
                break;
            case Customer.Difficulty.HARD:
                ModifyInsanity(hardInsanityPerSec * Time.deltaTime);
                break;
        }
    }

    public void ModifyInsanity(float value)
    {
        float newInsanity = Mathf.Max(0, insanity + value);
        insanity = newInsanity;
    }

    public float GetInsanity()
    {
        return insanity;
    }

    public void DebugLog(string input)
    {
        Debug.Log(input);
    }
}