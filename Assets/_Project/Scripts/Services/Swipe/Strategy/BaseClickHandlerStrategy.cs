using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseClickHandlerStrategy : IClickHandlerStrategy
{
    protected readonly IDisposable _observable;
    protected readonly Camera _camera;
    protected readonly List<GraphicRaycaster> _graphicRaycasters;
    protected readonly EventSystem _eventSystem;

    public event Action<IClickable, Vector2> Clicked;
    public event Action Unclicked;

    protected BaseClickHandlerStrategy()
    {
        _observable = Observable.EveryUpdate().Subscribe(_ => Update());
        _camera = Camera.main;
        _graphicRaycasters = UnityEngine.Object.FindObjectsOfType<GraphicRaycaster>().ToList();
        _eventSystem = EventSystem.current;
    }

    public abstract Vector2 GetCurrentPosition();

    protected abstract void Update();

    public void Dispose() =>
        _observable?.Dispose();

    protected bool IsUiElement(Vector2 position, out RaycastResult result)
    {
        result = new();

        if (_eventSystem == null || _graphicRaycasters.Count == 0)
            return false;

        PointerEventData eventData = new(_eventSystem) { position = position };
        RaycastResult topResult = new();
        bool foundAny = false;

        foreach (GraphicRaycaster raycaster in _graphicRaycasters)
        {
            List<RaycastResult> results = new();
            raycaster.Raycast(eventData, results);

            if (results.Count > 0)
            {
                if (foundAny == false || IsResultHigherPriority(results[0], topResult))
                {
                    topResult = results[0];
                    foundAny = true;
                }
            }
        }

        if (foundAny)
        {
            result = topResult;

            return true;
        }

        return false;
    }

    protected bool IsClickableUiElement(RaycastResult result, out IClickable clickable)
    {
        if (result.gameObject.TryGetComponent(out clickable))
            return true;

        clickable = result.gameObject.GetComponentInParent<IClickable>();

        return clickable != null;
    }

    protected void InvokeClicked(IClickable clickable, Vector2 position) =>
        Clicked?.Invoke(clickable, position);

    protected void InvokeUnclicked() =>
        Unclicked?.Invoke();

    private bool IsResultHigherPriority(RaycastResult newResult, RaycastResult currentTop)
    {
        Canvas newCanvas = newResult.gameObject.GetComponentInParent<Canvas>();
        Canvas currentCanvas = currentTop.gameObject.GetComponentInParent<Canvas>();

        int newOrder = newCanvas != null ? newCanvas.sortingOrder : 0;
        int currentOrder = currentCanvas != null ? currentCanvas.sortingOrder : 0;

        if (newOrder != currentOrder)
            return newOrder > currentOrder;

        return newResult.depth < currentTop.depth;
    }
}