using System;
using Unity.VisualScripting;
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

    void Update()
    {
        foreach (var @event in insanityEvents)
        {
            if (insanity >= @event.insanityTriggerValue)
            {
                @event.InvokeEvent();
            }
            else if(@event.hasTriggerd)
            {
                @event.hasTriggerd = false;
            }
        }
    }

    public float GetInsanity()
    {
        return insanity;
    }

    public void ModifyInsanity(float value)
    {
        insanity += value;
    }

    public void DebugLog(string input)
    {
        Debug.Log(input);
    }
}