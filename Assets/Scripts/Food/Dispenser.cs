using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public bool isDispensing = false;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ToggleDispenser()
    {
        isDispensing = !isDispensing;
        animator.SetBool("isDispensing", isDispensing);
    }
}
