using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    public Difficulty currentDifficulty = Difficulty.EASY;
    public CustomerState currentState = CustomerState.ARRIVING;
    
    private FoodReferenceTable foodReferenceTable;
    private GameObject iconPrefab;

    [SerializeField] private Animator animator;

    public int hungerLevel = 1;
    public List<FoodType> orderedMeals = new List<FoodType>();

    private GameObject orderUI;
    private TextMeshProUGUI infoText;

    private InputAction spaceBar; //For testing purpose only

    private void Start()
    {
        if (foodReferenceTable == null)
        {
            Debug.LogError("FoodReferenceTable is not assigned in Customer script.");
        }

        if (animator == null)
        {
            animator = this.GetComponent<Animator>();
        }

        spaceBar = InputSystem.actions.FindAction("Jump");

        orderUI.SetActive(false);
        infoText.gameObject.SetActive(true);

        StartCoroutine(Arrive());
    }

    public void InsertMembers(
        FoodReferenceTable _foodReferenceTable, GameObject _iconPrefab,
        GameObject _orderUI, TextMeshProUGUI _infoText
        )
    {
        foodReferenceTable = _foodReferenceTable;
        iconPrefab = _iconPrefab;
        orderUI = _orderUI;
        infoText = _infoText;
    }
    IEnumerator Arrive()
    {
        currentState = CustomerState.ARRIVING;
        // Initialize arrival behavior
        infoText.text = "Customer is arriving...";
        // > Arrival animation or effects
        // animator.Play("Customer_Arrive");
        // > Play arrival sound
        yield return new WaitForSeconds(2f);
        StartCoroutine(Order());
    }
    IEnumerator Order()
    {
        currentState = CustomerState.ORDERING;
        // Initialize ordering behavior
        infoText.text = "Customer is ordering...";
        // > Display order UI
        orderUI.SetActive(true);
        // > Set order icon
        GenerateOrder();
        // > Play ordering sound
        yield return new WaitForSeconds(2f);
        Wait();
    }

    void Wait()
    {
        currentState = CustomerState.WAITING;
        // Initialize waiting behavior
        infoText.text = "Customer is waiting for order...";
        // > Play waiting sound
    }

    IEnumerator Leave()
    {
        currentState = CustomerState.LEAVING;
        orderUI.SetActive(false);
        // Initialize leaving behavior
        infoText.text = "Customer is leaving...";
        // > Play leaving animation or effects
        // > Play leaving sound
        yield return new WaitForSeconds(2f);
        infoText.gameObject.SetActive(false);
        Destroy(gameObject); // Remove customer from scene
    }

    private void OnDestroy()
    {
        foreach (Transform child in orderUI.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
        if (currentState == CustomerState.WAITING)
        {
            if (spaceBar.WasPressedThisFrame())
            {
                StartCoroutine(Leave());
            }
        }
    }
    
    void GenerateOrder()
    {
        orderedMeals.Clear();
        
        for (int i = 0; i < hungerLevel; i++)
        {
            int randomIndex = Random.Range(1, System.Enum.GetValues(typeof(FoodType)).Length);
            orderedMeals.Add((FoodType)randomIndex);

            GameObject icon = Instantiate(iconPrefab);
            icon.transform.SetParent(orderUI.transform, false);
            icon.GetComponent<Image>().sprite = foodReferenceTable.GetSprite((FoodType)randomIndex);
        }
    }
}
