using UnityEngine;

public class BagShaker : FoodStationWorkArea
{
    [SerializeField] float dragResistance = 1;
    [SerializeField] float maxLength = 1;
    [SerializeField] float shakeToComplete;

    bool clicked = false;
    float currentShakeAmount;
    Vector2 mouseStart;
    Vector3 startPos;
    Vector3 lastPos;

    private void Awake()
    {
        startPos = transform.localPosition;
    }

    public override void OnClick()
    {
        clicked = true;
        lastPos = startPos;
        mouseStart = Clicker.MousePos;
    }

    public override void OnReleaseAny()
    {
        transform.localPosition = startPos;
        clicked = false;
    }

    private void FixedUpdate()
    {
        if (clicked)
        {
            Vector2 offset = Clicker.MousePos - mouseStart;
            float magnitude = offset.magnitude;
            float length = dragResistance * magnitude;
            length = Mathf.Min(length, maxLength);
            offset /= Mathf.Max(magnitude, Mathf.Epsilon);
            transform.localPosition = startPos + (Vector3)(offset * length);

            currentShakeAmount += (lastPos - transform.localPosition).sqrMagnitude;
            if (currentShakeAmount > Mathf.Pow(shakeToComplete, 2))
            {
                FoodReferenceTable.Instance.SpawnFood(foodType, transform.position);
                Deactivate();
            }
        }
    }
}
