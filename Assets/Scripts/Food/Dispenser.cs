using UnityEngine;
using UnityEngine.UI;

public class Dispenser : FoodStationWorkArea
{
    private Animator animator;
    [SerializeField] private Image cupFillImage;
    //public GameObject dispenserWindow;
    
    public bool isDispensing = false;
    
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

                Deactivate();
                Invoke(nameof(ResetWorkstation), glideOut.length);
            }
        }
    }

    void ResetWorkstation()
    {
        isDispensing = false;
        animator.SetBool("isDispensing", false);
        cupFillImage.fillAmount = 0;
        //dispenserWindow.GetComponent<Animator>().SetTrigger("Fade");
    }
}
