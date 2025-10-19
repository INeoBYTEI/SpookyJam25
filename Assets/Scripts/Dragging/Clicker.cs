using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Clicker : MonoBehaviour
{
    [SerializeField] InputAction action;

    HashSet<Clickable> clickedClickables = new HashSet<Clickable>();
    HashSet<Clickable> hoverClickables = new HashSet<Clickable>();


    public static Clicker Instance { get; private set; }
    public static Vector2 MousePos { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(this); }
    }

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
            clickable.Hover();
            newHovering.Add(clickable);
        });

        foreach (Clickable clickedClickable in clickedClickables)
        {
            if (clickedClickable) { clickedClickable.OnDrag(); }
        }

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
        MousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Collider2D[] hits = Physics2D.OverlapPointAll(MousePos);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.TryGetComponent<Clickable>(out Clickable clickable))
            {
                if (!clickedClickables.Contains(clickable)) clickedClickables.Add(clickable);
                clickable.OnClick();
                return;
            }
        }
    }

    private void ReleaseClick(InputAction.CallbackContext context)
    {
        GetClickables((clickable) =>
        {
            clickable.OnRelease();
            if (clickedClickables.Contains(clickable)) { clickable.OnReleaseSame(); }
        });

        foreach (Clickable clickable in clickedClickables)
        {
            if (clickable) { clickable.OnReleaseAny(); }
        }

        clickedClickables.Clear();
    }

    private void GetClickables(Action<Clickable> action)
    {
        MousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Collider2D[] hits = Physics2D.OverlapPointAll(MousePos);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.TryGetComponent<Clickable>(out Clickable clickable))
            {
                action(clickable);
            }
        }
    }

    public void AddClicked(Clickable clickable)
    {
        clickedClickables.Add(clickable);
    }

    private void OnDisable()
    {
        action.started -= Click;
        action.canceled -= ReleaseClick;
    }
}
