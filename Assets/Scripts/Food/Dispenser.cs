using UnityEngine;
using UnityEngine.UI;

public class Dispenser : MonoBehaviour
{
    public bool isDispensing = false;
    [SerializeField] private Image cupFillImage;
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

    void Update()
    {
        if (isDispensing)
        {
            cupFillImage.fillAmount += Time.deltaTime * 0.5f;
        }
    }
}
