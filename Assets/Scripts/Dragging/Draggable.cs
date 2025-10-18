using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Draggable : Clickable
{
    Vector2 offset;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnClick()
    {
        rb.gravityScale = 0;
        offset = new Vector2(transform.position.x, transform.position.y) - Clicker.MousePos;
    }

    public override void OnDrag()
    {
        transform.position = Clicker.MousePos + offset;
    }

    public override void OnReleaseSame()
    {
        rb.gravityScale = 1;
        rb.linearVelocity = Vector2.zero;
    }
}
