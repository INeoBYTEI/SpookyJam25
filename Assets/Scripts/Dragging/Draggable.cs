using UnityEngine;

public class Draggable : Clickable
{
    Vector2 offset;

    public override void OnClick()
    {
        offset = new Vector2(transform.position.x, transform.position.y) - Clicker.MousePos;
    }

    public override void OnDrag()
    {
        transform.position = Clicker.MousePos + offset;
    }
}
