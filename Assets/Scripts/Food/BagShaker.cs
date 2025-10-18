using UnityEngine;

public class BagShaker : FoodStationWorkArea
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] SpriteRenderer[] fries;
    [SerializeField] float dragResistance = 1;
    [SerializeField] float maxLength = 1;
    [SerializeField] float shakeToComplete;
    [SerializeField] Color FinishedColor;

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

            float shakeIntesity = (lastPos - transform.localPosition).sqrMagnitude;
            currentShakeAmount += shakeIntesity;

            if (shakeIntesity > 0.9f)
            {
                AudioManager.Instance.PlayAudio("ShakeBag", default, default, shakeIntesity, default, default, false);
            }
            
            if (currentShakeAmount > Mathf.Pow(shakeToComplete, 2))
            {
                FoodReferenceTable.Instance.SpawnFood(foodType, transform.position);
                currentShakeAmount = 0;
                Deactivate();
            }

            float value = curve.Evaluate(currentShakeAmount / Mathf.Pow(shakeToComplete, 2));
            foreach (SpriteRenderer spriteRenderer in fries)
            {
                spriteRenderer.color = Vector4.Lerp(Color.white, FinishedColor, value);
            }
        }
    }
}