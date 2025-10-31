using System;
using UniRx;
using UnityEngine;

public class PCSwipeClickHandlerStrategy : IClickHandlerStrategy
{
    private readonly IDisposable _updateSubscription;
    private readonly Camera _camera;

    public PCSwipeClickHandlerStrategy()
    {
        _updateSubscription = Observable.EveryUpdate().Subscribe(_ => Update());
        _camera = Camera.main;
    }

    public event Action<ISwipeable, Vector2> Clicked;
    public event Action Unclicked;

    public void Dispose() =>
        _updateSubscription?.Dispose();

    public Vector2 GetCurrentPosition() =>
        Input.mousePosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;

            Ray ray = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo) == false)
                return;

            if (hitInfo.collider.TryGetComponent(out Car car))
                Clicked?.Invoke(car, mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
            Unclicked?.Invoke();
    }
}

public interface IClickHandlerStrategy : IDisposable
{
    event Action<ISwipeable, Vector2> Clicked;
    event Action Unclicked;

    Vector2 GetCurrentPosition();
}

public interface ISwipeable : IClickable 
{ 
    Transform Transform { get; }
}

public interface IClickable { }