using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    IEnumerator Order()
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
