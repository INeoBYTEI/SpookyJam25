using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq.Expressions;

public class CustomerHandler : MonoBehaviour
{
    public static CustomerHandler Instance { get; private set; }
    [SerializeField] private List<Customer> customerVariants = new List<Customer>();

    public float spawnInterval = 3f;
    public int customersSpawned = 0;
    public int customersServed = 0;
    public int karenSpawnChance = 0;

    private float spawnTimer;
    public Customer currentCustomer;
    public static Action<Customer> OnCustomerSpawned;
    [SerializeField] private FoodReferenceTable foodReferenceTable;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private GameObject orderUI;
    [SerializeField] private TextMeshProUGUI infoText;
    void Start()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(this); }

        SpawnCustomer(0, 1);
    }
    void SpawnCustomer(int index = -1, int hungerLevel = -1)
    {
        customersSpawned++;

        if (index == -1)
        {
            index = UnityEngine.Random.Range(0, customerVariants.Count);
        }

        if (hungerLevel == -1)
        {
            hungerLevel = UnityEngine.Random.Range(1, 4);
        }

        currentCustomer = Instantiate(customerVariants[index], Vector3.zero, Quaternion.identity);
        currentCustomer.InsertMembers(foodReferenceTable, iconPrefab, orderUI, infoText);
        currentCustomer.hungerLevel = hungerLevel;
        OnCustomerSpawned?.Invoke(currentCustomer);

        spawnTimer = spawnInterval;
        karenSpawnChance += 5;
    }

    public void HideUI()
    {
        orderUI.SetActive(false);
        infoText.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        orderUI.SetActive(true);
        infoText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (currentCustomer == null)
        {

            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                int index = UnityEngine.Random.Range(0, 100);
                if (index < karenSpawnChance)
                {
                    SpawnCustomer(3);
                    karenSpawnChance = 0;
                }
                else
                {
                    index %= 3;
                    SpawnCustomer(index);
                }
                Debug.Log(index);
                /* [Archived] Slow Progression of difficulty
                else if (customersServed < 2)
                {
                    SpawnCustomer(0);
                }
                else if (customersServed < 7)
                {
                    index %= 2;
                    SpawnCustomer(index);
                }
                else if (customersServed < 15)
                {
                    index %= 2;
                    index += 1;
                    SpawnCustomer(index);
                }
                else
                {
                    index %= 3;
                    SpawnCustomer(index);
                }
                */
            }
        }  
    }
}
