using UnityEngine;
using UnityEngine.U2D;

public class FoodStationStart : Clickable
{
    [SerializeField] FoodType foodType;
    [SerializeField] GameObject workAreaPrefab;
    FoodStationWorkArea workArea;

    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite normal;
    [SerializeField] Sprite clicked;

    static bool active = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        workArea = Instantiate(workAreaPrefab).GetComponentInChildren<FoodStationWorkArea>(true);
        workArea.stationStart = this;
    }

    public override void OnClick()
    {   
        if (workArea.transform.root.gameObject.activeSelf || active) { return; }
        active = true;
        workArea.Activate(foodType);
        spriteRenderer.sprite = clicked;
    }

    public void FullyDethatched()
    {
        active = false;
    }

    public override void OnReleaseAny()
    {
        spriteRenderer.sprite = normal;
    }
}
