using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Clicker : MonoBehaviour
{
    [SerializeField] InputAction action;

    HashSet<Clickable> clickedClickables = new HashSet<Clickable>();
    HashSet<Clickable> hoverClickables = new HashSet<Clickable>();

    bool mouseDown;

    private void OnEnable()
    {
        action.Enable();
        action.started += Click;
        action.canceled += ReleaseClick;
    }

    private void Update()
    {
        HashSet<Clickable> newHovering = new HashSet<Clickable>();

        GetClickables((clickable) =>
        {
            if (mouseDown && clickedClickables.Contains(clickable)) { clickable.OnDrag(); }
            else { clickable.Hover(); }
            newHovering.Add(clickable);
        });

        foreach (Clickable clickable in hoverClickables)
        {
            if (!newHovering.Contains(clickable))
            {
                clickable.HoverLeave();
            }
        }

        hoverClickables = newHovering;
    }

    private void Click(InputAction.CallbackContext context)
    {
        mouseDown = true;

        GetClickables((clickable) =>
        {
            clickable.OnClick();
            if (!clickedClickables.Contains(clickable)) clickedClickables.Add(clickable);
        });
    }

    private void ReleaseClick(InputAction.CallbackContext context)
    {
        mouseDown = false;

        GetClickables((clickable) =>
        {
            if (clickedClickables.Contains(clickable)) { clickable.OnReleaseSame(); }
            else { clickable.OnRelease(); }
        });

        clickedClickables.Clear();
    }

    private void GetClickables(Action<Clickable> action)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.TryGetComponent<Clickable>(out Clickable clickable))
            {
                action(clickable);
            }
        }
    }

    private void OnDisable()
    {
        action.started -= Click;
        action.canceled -= ReleaseClick;
    }
}
