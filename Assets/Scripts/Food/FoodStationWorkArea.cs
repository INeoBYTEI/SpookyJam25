using UnityEngine;

public class FoodStationWorkArea : Clickable
{
    [SerializeField] protected AnimationClip glideIn;
    [SerializeField] protected AnimationClip glideOut;

    Animator transitionAnimator;
    [HideInInspector] public FoodStationStart stationStart;
    protected FoodType foodType;

    [SerializeField] GameObject cover;

    private void Start()
    {
        transitionAnimator = transform.root.GetComponentInChildren<Animator>();
        transform.root.gameObject.SetActive(false);
    }

    public virtual void Activate(FoodType foodType)
    {
        this.enabled = true;
        this.foodType = foodType;
        transform.root.gameObject.SetActive(true);
        cover.SetActive(true);
        transitionAnimator.Play(glideIn.name);

        CustomerHandler.Instance.HideUI();
    }

    public void Deactivate()
    {
        transitionAnimator.Play(glideOut.name);
        this.enabled = false;
        cover.SetActive(false);

        Invoke(nameof(DeactivateSelf), glideOut.length);
    }

    protected virtual void DeactivateSelf()
    {
        CustomerHandler.Instance.ShowUI();
        stationStart.FullyDethatched();
        transform.root.gameObject.SetActive(false);
    }
}
