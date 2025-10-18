using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerHandler : MonoBehaviour
{
    public static CustomerHandler Instance { get; private set; }
    [SerializeField] private List<Customer> customerVariants = new List<Customer>();

    public float spawnInterval = 3f;
    public int customersSpawned = 0;
    public int customersServed = 0;

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

        currentCustomer = Instantiate(customerVariants[index]);
        currentCustomer.InsertMembers(foodReferenceTable, iconPrefab, orderUI, infoText);
        currentCustomer.hungerLevel = hungerLevel;
        OnCustomerSpawned?.Invoke(currentCustomer);

        spawnTimer = spawnInterval;
    }
    void Update()
    {
        if (currentCustomer == null)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                if (customersSpawned >= 10)
                {
                    SpawnCustomer(2, UnityEngine.Random.Range(2, 4));
                }
                else if (customersSpawned >= 7)
                {
                    SpawnCustomer(2, 1);
                }
                else if (customersSpawned >= 5)
                {
                    SpawnCustomer(1, UnityEngine.Random.Range(2, 4));
                }
                else if (customersSpawned >= 3)
                {
                    SpawnCustomer(1, 1);
                }
                else
                {
                    SpawnCustomer(0);
                }
            }
        }  
    }
}
