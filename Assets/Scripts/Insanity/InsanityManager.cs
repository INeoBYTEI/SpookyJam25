using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public static InsanityManager Instance;

    [SerializeField] float easyInsanityPerSec = 1;
    [SerializeField] float mediumInsanityPerSec = 2;
    [SerializeField] float hardInsanityPerSec = 3;
    [SerializeField] float karenInsanityPerSec = 4;
    public float karenOrderCompleteInsanity = 10;
    [SerializeField] float insanityRecoverPerSec = 1;

    [Header("insanityView")]
    [SerializeField] Image insanityOverlayImage;
    [SerializeField] Sprite[] insanityFrames;

    void Start()
    {
        CustomerHandler.OnCustomerSpawned += SetCurrentCustomer;
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        UpdateInsanityOverlay();
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
            case Customer.Difficulty.KAREN:
                ModifyInsanity(karenInsanityPerSec * Time.deltaTime);
                break;
        }
    }

    public void ModifyInsanity(float value)
    {
        float newInsanity = Mathf.Max(0, insanity + value);
        insanity = newInsanity;
        UpdateInsanityOverlay();
    }

    void UpdateInsanityOverlay() //100 is max insanity before you loose, just cant be botherd to make a variable for it
    {
        int frameIndex = Mathf.RoundToInt((insanity / 100f) * (insanityFrames.Length - 1));
        frameIndex = Mathf.Clamp(frameIndex, 0, insanityFrames.Length - 1);

        Debug.Log(frameIndex);
        insanityOverlayImage.sprite = insanityFrames[frameIndex];
        insanityOverlayImage.color = Color.Lerp(Color.clear, Color.white, insanity / 100f);
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