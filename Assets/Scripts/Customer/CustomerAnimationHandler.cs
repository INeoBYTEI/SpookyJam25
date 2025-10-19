using System.Collections;
using UnityEngine;

public class CustomerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator carAnimator;
    [SerializeField] private Animator monsterAnimator;
    [SerializeField] private Customer customer;

    private enum CarColor
    {
        BLUE, GREEN, YELLOW
    }

    CarColor carColor;


    void Start()
    {
        carColor = (CarColor)Random.Range(0, 3);

        switch (carColor)
        {
            case CarColor.BLUE:
                carAnimator.Play("Blue_Car_Arrive");
                break;
            case CarColor.GREEN:
                carAnimator.Play("Green_Car_Arrive");
                break;
            case CarColor.YELLOW:
                carAnimator.Play("Yellow_Car_Arrive");
                break;
        }

        switch (customer.currentDifficulty)
        {
            case Customer.Difficulty.EASY:
                monsterAnimator.Play("Dark");
                break;
            case Customer.Difficulty.MEDIUM:
                monsterAnimator.Play("Gubb");
                break;
            case Customer.Difficulty.HARD:
                monsterAnimator.Play("Squid");
                break;
            case Customer.Difficulty.KAREN:
                monsterAnimator.Play("Squid");
                break;
        }
    }
    
    public IEnumerator PlayLeaveAnimation()
    {
        monsterAnimator.SetTrigger("Leave");
        yield return new WaitForSeconds(.75f);
        carAnimator.SetTrigger("Leave");
    }

}
