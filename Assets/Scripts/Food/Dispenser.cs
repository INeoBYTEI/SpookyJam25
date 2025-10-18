using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public bool isDispensing = false;
    private Animator animator;
    void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    public void ToggleDispenser()
    {
        isDispensing = !isDispensing;
        animator.SetBool("isDispensing", isDispensing);
    }
}
