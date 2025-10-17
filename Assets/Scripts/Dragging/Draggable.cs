using UnityEngine;

public class Draggable : Clickable
{
    public override void OnClick()
    {
        Debug.Log("OnClick");
    }

    public override void OnDrag()
    {
        Debug.Log("OnDrag");
    }

    public override void Hover()
    {
        Debug.Log("hover");
    }

    public override void OnRelease()
    {
        Debug.Log("OnRelease");
    }

    public override void OnReleaseSame()
    {
        Debug.Log("OnReleaseSame");
    }

    public override void HoverLeave()
    {
        Debug.Log("HoverLeave");
    }
}
