using System;
using UniRx;
using UnityEngine;

public class MobileClickHandlerStrategy : IClickHandlerStrategy, IDisposable
{
    private readonly IDisposable _updateSubscription;
    private readonly Camera _camera;

    public MobileClickHandlerStrategy()
    {
        _updateSubscription = Observable.EveryUpdate().Subscribe(_ => Update());
        _camera = Camera.main;
    }

    public event Action<ISwipeable, Vector2> Clicked;
    public event Action Unclicked;

    public void Dispose() =>
        _updateSubscription?.Dispose();

    public Vector2 GetCurrentPosition() =>
        Input.GetTouch(0).position;

    private void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            Vector2 touchPosition = touch.position;
            Ray ray = _camera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo) == false)
                return;

            if (hitInfo.collider.TryGetComponent(out Car car))
                Clicked?.Invoke(car, touchPosition);
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            Unclicked?.Invoke();
    }
}