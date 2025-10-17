using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public enum CustomerState
    {
        ARRIVING,
        ORDERING,
        WAITING,
        LEAVING
    }

    [SerializeField] private CustomerState currentState = CustomerState.ARRIVING;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject orderUI;
    [SerializeField] private GameObject orderIconUI;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Sprite orderIcon;
    private InputAction spaceBar;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        spaceBar = InputSystem.actions.FindAction("Jump");

        orderUI.SetActive(false);
        infoText.gameObject.SetActive(true);

        currentState = CustomerState.ARRIVING;
        StartCoroutine(Arrive());
    }

    IEnumerator Arrive()
    {
        // Initialize arrival behavior
        infoText.text = "Customer is arriving...";
        // > Arrival animation or effects
        // animator.Play("Customer_Arrive");
        // > Play arrival sound
        yield return new WaitForSeconds(2f);
        currentState = CustomerState.ORDERING;
        StartCoroutine(Order());
    }
    IEnumerator Order()
    {
        // Initialize ordering behavior
        infoText.text = "Customer is ordering...";
        // > Display order UI
        orderUI.SetActive(true);
        // > Set order icon
        orderIconUI.GetComponent<Image>().sprite = orderIcon;

        // > Play ordering sound
        yield return new WaitForSeconds(5f);
        orderUI.SetActive(false);
        currentState = CustomerState.WAITING;
        Wait();
    }

    void Wait()
    {
        // Initialize waiting behavior
        infoText.text = "Customer is waiting for order...";
        // > Start waiting timer
        // > Play waiting sound
    }

    IEnumerator Leave()
    {
        currentState = CustomerState.LEAVING;
        // Initialize leaving behavior
        infoText.text = "Customer is leaving...";
        // > Play leaving animation or effects
        // > Play leaving sound
        yield return new WaitForSeconds(2f);
        infoText.gameObject.SetActive(false);
        Destroy(gameObject); // Remove customer from scene
    }
    void Update()
    {
        if(currentState == CustomerState.WAITING)
        {
            if (spaceBar.WasPressedThisFrame())
            {
                StartCoroutine(Leave());
            }
        }  
    }
}
