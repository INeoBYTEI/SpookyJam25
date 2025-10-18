using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHandler : MonoBehaviour
{

    [SerializeField] private List<Customer> customerVariants = new List<Customer>();
    public Customer currentCustomer;
    public Action<Customer> OnCustomerSpawned;
    void SpawnCustomer(int index = -1)
    {
        if (index == -1)
        {
            index = UnityEngine.Random.Range(0, customerVariants.Count);
        }

        currentCustomer = Instantiate(customerVariants[index]);
        OnCustomerSpawned?.Invoke(currentCustomer);
    }

}
