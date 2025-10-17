using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        currentState = CustomerState.ARRIVING;
        StartCoroutine(Arrive());
    }

    IEnumerator Arrive()
    {
        // Initialize arrival behavior
        // > Arrival animation or effects
        // animator.Play("Customer_Arrive");
        // > Play arrival sound
        yield return new WaitForSeconds(1f);
        currentState = CustomerState.ORDERING;
    }
    IEnumerator Order()
    {
        // Initialize ordering behavior
        // > Display order UI
        // > Play ordering sound
        yield return new WaitForSeconds(1f);
        currentState = CustomerState.WAITING;
    }

    IEnumerator Wait()
    {
        // Initialize waiting behavior
        // > Start waiting timer
        // > Play waiting sound
        yield return new WaitForSeconds(1f);
        currentState = CustomerState.LEAVING;
    }

    IEnumerator Leave()
    {
        // Initialize leaving behavior
        // > Play leaving animation or effects
        // > Play leaving sound
        yield return new WaitForSeconds(1f);
        Destroy(gameObject); // Remove customer from scene
    }
    void Update()
    {
        if(currentState == CustomerState.WAITING)
        {
            
        }  
    }
}
