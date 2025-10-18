using UnityEngine;
using UnityEngine.UI;

public class Dispenser : MonoBehaviour
{
    public bool isDispensing = false;
    public GameObject dispenserWindow;
    private FoodType foodType = FoodType.Soda;
    [SerializeField] private Image cupFillImage;
    private Animator animator;
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        cupFillImage.fillAmount = 0;
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

            if (cupFillImage.fillAmount >= 1f)
            {
                ToggleDispenser();
                FoodReferenceTable.Instance.SpawnFood(foodType, transform.position);
                
                Invoke("Deactivate", 0.2f);
            }
        }
    }

    void Deactivate()
    {
        isDispensing = false;
        animator.SetBool("isDispensing", false);
        cupFillImage.fillAmount = 0;
        dispenserWindow.GetComponent<Animator>().SetTrigger("Fade");
    }
}
