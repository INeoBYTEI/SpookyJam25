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
        if (CustomerHandler.Instance.currentCustomer.currentState == Customer.CustomerState.LEAVING ||
        CustomerHandler.Instance.currentCustomer.currentState == Customer.CustomerState.ARRIVING)
        {
            return;
        }

        if (CustomerHandler.Instance.currentCustomer != null)
        {
            InsanityManager.Instance.ModifyInsanity(5);
        }

        CustomerHandler.Instance.currentCustomer.ForceLeave();
    }

    public override void OnReleaseSame()
    {
        animator.Play(noSprayClip.name);
    }
}
