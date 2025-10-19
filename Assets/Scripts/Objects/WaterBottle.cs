using System;
using UnityEngine;

public class WaterBottle : Clickable
{
    Animator animator;
    [SerializeField] AnimationClip noSprayClip;
    [SerializeField] AnimationClip sprayClip;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnClick()
    {
        animator.Play(sprayClip.name);
        AudioManager.Instance.PlayAudio("Spray", null, false, 1, true, 0, true);
        Customer currentCustomer = CustomerHandler.Instance.currentCustomer;
        if (currentCustomer.currentState == Customer.CustomerState.LEAVING ||
        currentCustomer.currentState == Customer.CustomerState.ARRIVING)
        {
            return;
        }

        if (currentCustomer.currentDifficulty == Customer.Difficulty.KAREN)
        {
            currentCustomer.ForceLeave();
            InsanityManager.Instance.ModifyInsanity(-10);
        }
        else
        {
            InsanityManager.Instance.ModifyInsanity(5);
            currentCustomer.PlayCustomerVoiceline(currentCustomer.currentDifficulty, Customer.CustomerEmotion.ANGRY);
        }

    }

    public override void OnReleaseSame()
    {
        animator.Play(noSprayClip.name);
    }
}
