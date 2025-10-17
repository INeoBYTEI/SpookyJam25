using UnityEngine;
using System.Collections.Generic;
using System;

public class FoodReferenceTable : MonoBehaviour
{
    [SerializeField] List<FoodPrefabPair> referenceTable = new();

    public static FoodReferenceTable Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(this); }
    }

    public GameObject Spawn(FoodType foodType, Vector3 position)
    {
        GameObject gameObject = Instantiate(referenceTable[(int)foodType].prefab, position, Quaternion.identity);
        if ( gameObject.TryGetComponent<Food>(out Food food))
        {
            food.Type = foodType;
        }

        return gameObject;
    }
}

[Serializable]
public struct FoodPrefabPair
{
    public GameObject prefab;
    public FoodType foodType;
}

public enum FoodType
{
    Nothing,
    Burger,
    tentacle,
    bread
}
