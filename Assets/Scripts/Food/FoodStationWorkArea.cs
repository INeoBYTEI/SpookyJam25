using UnityEngine;

public class FoodStationWorkArea : Clickable
{
    [SerializeField] AnimationClip glideIn;
    [SerializeField] AnimationClip glideOut;

    Animator animator;
    [HideInInspector] public FoodStationStart stationStart;
    protected FoodType foodType;

    [SerializeField] GameObject cover;

    private void Start()
    {
        animator = transform.root.GetComponentInChildren<Animator>();
        transform.root.gameObject.SetActive(false);
    }

    public virtual void Activate(FoodType foodType)
    {
        this.enabled = true;
        this.foodType = foodType;
        transform.root.gameObject.SetActive(true);
        cover.SetActive(true);
        animator.Play(glideIn.name);

        CustomerHandler.Instance.HideUI();
    }

    public void Deactivate()
    {
        animator.Play(glideOut.name);
        this.enabled = false;
        cover.SetActive(false);

        Invoke(nameof(DeactivateSelf), glideOut.length);
    }

    private void DeactivateSelf()
    {
        CustomerHandler.Instance.ShowUI();
        transform.root.gameObject.SetActive(false);
    }
}
