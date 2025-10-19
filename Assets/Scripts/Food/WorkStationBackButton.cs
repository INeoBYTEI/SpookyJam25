using UnityEngine;

public class WorkStationBackButton : Clickable
{
    FoodStationWorkArea workArea;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite normal;
    [SerializeField] Sprite clicked;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        workArea = transform.root.GetComponentInChildren<FoodStationWorkArea>();
    }

    public override void OnClick()
    {
        if (workArea) { workArea.Deactivate(); }
        spriteRenderer.sprite = clicked;
    }

    public override void OnReleaseAny()
    {
        spriteRenderer.sprite = normal;
    }
}
