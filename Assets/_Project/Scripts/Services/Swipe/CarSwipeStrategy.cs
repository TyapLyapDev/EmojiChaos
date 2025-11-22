using System;
using UniRx;
using UnityEngine;

public class CarSwipeStrategy : ISwipeStrategy, IDisposable
{
    private const float SwipeDistance = 20f;

    private readonly IClickHandlerStrategy _clickHandler;
    private readonly Camera _camera;

    private IDisposable _updateSubscription;
    private ISwipeable _swipeableObject;
    private Vector2 _startPosition;

    public CarSwipeStrategy()
    {
        _clickHandler = CreatePlatformSpecificStrategy();
        _camera = Camera.main;

        _clickHandler.Clicked += OnClick;
        _clickHandler.Unclicked += OnUnclick;
    }

    public event Action<ISwipeable, int> HasSwipe;

    public void Dispose()
    {
        if (_clickHandler is IDisposable disposable)
            disposable.Dispose();
    }

    private IClickHandlerStrategy CreatePlatformSpecificStrategy()
    {
        if (Utils.IsMobilePlatform())
            return new MobileClickHandlerStrategy();
        else
            return new PCClickHandlerStrategy();
    }

    private void OnClick(IClickable clickableObject, Vector2 startPosition)
    {
        if(clickableObject is ISwipeable swipeableObject)

        _swipeableObject = swipeableObject ?? throw new ArgumentNullException(nameof(swipeableObject));
        _startPosition = startPosition;

        _updateSubscription = Observable.EveryUpdate().Subscribe(_ => HandleSwipeState());
    }

    private void OnUnclick()
    {
        _swipeableObject = null;
        _startPosition = Vector2.zero;
        _updateSubscription?.Dispose();
    }

    private void HandleSwipeState()
    {
        Vector2 delta = _clickHandler.GetCurrentPosition() - _startPosition;
        float distance = delta.magnitude;

        if (distance > SwipeDistance)
        {
            Vector3 worldDirection = Utils.ConvertScreenSwipeToWorldDirection(delta.normalized, _camera.transform);
            int specificDirection = Utils.CalculateSwipeDirectionRelativeToCar(worldDirection, _swipeableObject.Transform);

            if (specificDirection != 0)
            {
                HasSwipe?.Invoke(_swipeableObject, specificDirection);
                OnUnclick();
            }
        }
    }
}