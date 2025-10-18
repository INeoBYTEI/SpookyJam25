using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHandler : MonoBehaviour
{

    [SerializeField] private List<Customer> customerVariants = new List<Customer>();
    public float spawnInterval = 5f;
    private float spawnTimer;
    public Customer currentCustomer;
    public static Action<Customer> OnCustomerSpawned;
    void Start()
    {
        SpawnCustomer();
    }
    void SpawnCustomer(int index = -1)
    {
        if (index == -1)
        {
            index = UnityEngine.Random.Range(0, customerVariants.Count);
        }

        currentCustomer = Instantiate(customerVariants[index]);
        OnCustomerSpawned?.Invoke(currentCustomer);

        spawnTimer = spawnInterval;
    }
    void Update()
    {
        if (currentCustomer == null)
        {
            spawnInterval -= Time.deltaTime;
            if (spawnInterval <= 0f)
            {
                SpawnCustomer();
            }
        }  
    }
}
