using UnityEngine;

public class FoodStationWorkArea : Clickable
{
    [SerializeField] AnimationClip glideIn;
    [SerializeField] AnimationClip glideOut;

    Animator animator;
    [HideInInspector] public FoodStationStart stationStart;
    protected FoodType foodType;

    private void Start()
    {
        animator = transform.root.GetComponentInChildren<Animator>();
        transform.root.gameObject.SetActive(false);
    }

    public void Activate(FoodType foodType)
    {
        this.enabled = true;
        this.foodType = foodType;
        transform.root.gameObject.SetActive(true);
        animator.Play(glideIn.name);
    }

    public void Deactivate()
    {
        animator.Play(glideOut.name);
        this.enabled = false;
        Invoke(nameof(DeactivateSelf), glideOut.length);
    }

    private void DeactivateSelf() { transform.root.gameObject.SetActive(false); }
}
