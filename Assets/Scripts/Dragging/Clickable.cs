using UnityEngine;

public class Clickable : MonoBehaviour
{
    public virtual void OnClick() { }

    public virtual void OnDrag() { }

    public virtual void OnRelease() { }

    public virtual void OnReleaseSame() { }

    public virtual void OnReleaseAny() { }

    public virtual void Hover() { }

    public virtual void HoverLeave() { }
}
