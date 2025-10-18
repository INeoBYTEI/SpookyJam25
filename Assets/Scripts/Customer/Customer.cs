using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
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
        HARD,
        KAREN
    }

    public Difficulty currentDifficulty = Difficulty.EASY;
    public CustomerState currentState = CustomerState.ARRIVING;

    private FoodReferenceTable foodReferenceTable;
    private GameObject iconPrefab;

    [SerializeField] private Animator animator;

    public int hungerLevel = 1;
    public List<FoodOrder> orderedMeals = new();
    public List<FoodOrder> orderedMealsBackup = new();

    private GameObject orderUI;
    private TextMeshProUGUI infoText;

    private InputAction spaceBar; //For testing purpose only
    AudioSource carSource;

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
        carSource = GetComponent<AudioSource>();

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
        AudioManager.Instance.PlayAudio("CarArrive", carSource, false, 1, true, 0, true);
        // > Play arrival sound
        yield return new WaitForSeconds(carSource.clip.length);
        StartCoroutine(Order());
    }
    public IEnumerator Order()
    {
        currentState = CustomerState.ORDERING;
        AudioManager.Instance.PlayAudio("CarIdle", carSource, true, 1, true, 0, true);
        // Initialize ordering behavior
        infoText.text = "Customer is ordering...";
        // > Display order UI
        orderUI.SetActive(true);
        // > Set order icon
        GenerateOrder();
        yield return new WaitForSeconds(carSource.clip.length);
        Wait();
    }

    void Wait()
    {
        currentState = CustomerState.WAITING;
        // Initialize waiting behavior
        infoText.text = "Customer is waiting for order...";
        // > Play waiting sound
    }

    public void ForceLeave()
    {
        StopAllCoroutines();
        StartCoroutine(Leave());
    }

    public IEnumerator Leave()
    {
        currentState = CustomerState.LEAVING;
        orderUI.SetActive(false);
        // Initialize leaving behavior
        infoText.text = "Customer is leaving...";
        // > Play leaving animation or effects
        AudioManager.Instance.PlayAudio("CarLeave", carSource, false, 1, true, 0, true);
        // > Play leaving sound
        yield return new WaitForSeconds(carSource.clip.length);
        infoText.gameObject.SetActive(false);
        foreach (Transform child in orderUI.transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(gameObject); // Remove customer from scene
    }

    public IEnumerator KarenReorder()
    {
        foreach (var order in orderedMealsBackup)
        {
            order.icon.transform.GetChild(0).gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(3);
        foreach (var order in orderedMealsBackup)
        {
            Destroy(order.icon);
        }
        StartCoroutine(Order());
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
        orderedMealsBackup.Clear();

        int randomIndex = Random.Range(1, Enum.GetValues(typeof(FoodType)).Length);
        for (int i = 0; i < hungerLevel; i++)
        {
            orderedMeals.Add(FixFoodOrder((FoodType)randomIndex));
        }
        orderedMealsBackup = new(orderedMeals);
    }

    FoodOrder FixFoodOrder(FoodType type)
    {
        FoodOrder foodOrder = new();
        foodOrder.type = type;
        foodOrder.icon = Instantiate(iconPrefab);
        foodOrder.icon.transform.SetParent(orderUI.transform, false);
        foodOrder.icon.GetComponent<Image>().sprite = foodReferenceTable.GetSprite(type);
        return foodOrder;
    }
}

public class FoodOrder
{
    public FoodType type;
    public GameObject icon;
}
