using System;
using UnityEngine;

public class WaterBottle : Clickable
{
    Animator animator;
    [SerializeField] AnimationClip noSprayClip;
    [SerializeField] AnimationClip sprayClip;
    public Action Sprayed;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnClick()
    {
        animator.Play(sprayClip.name);
    }

    public override void OnReleaseSame()
    {
        animator.Play(noSprayClip.name);
        Sprayed?.Invoke();
    }
}
