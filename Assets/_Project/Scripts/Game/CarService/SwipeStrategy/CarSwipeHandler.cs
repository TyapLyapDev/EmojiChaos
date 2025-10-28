using System;
using UniRx;
using UnityEngine;

public class CarSwipeHandler : IDisposable
{
    private const float SwipeDistance = 20f;

    private readonly ICarClickHandlerStrategy _carClickHandler;
    private readonly Camera _camera;

    private IDisposable _updateSubscription;
    private Car _car;
    private Vector2 _startPosition;

    public CarSwipeHandler()
    {
        _carClickHandler = CreatePlatformSpecificStrategy();
        _camera = Camera.main;

        _carClickHandler.Clicked += OnCarClick;
        _carClickHandler.Unclicked += OnCarUnclick;
    }

    public event Action<Car, int> HasSwipe;

    public void Dispose()
    {
        _carClickHandler.Dispose();
    }

    private ICarClickHandlerStrategy CreatePlatformSpecificStrategy()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return new PCCarClickHandlerStrategy();
#elif UNITY_IOS || UNITY_ANDROID
        return new MobileCarClickHandlerStrategy();
#else
        return new PCCarClickHandlerStrategy();
#endif
    }

    private void OnCarClick(Car car, Vector2 startPosition)
    {
        _car = car;
        _startPosition = startPosition;

        _updateSubscription = Observable.EveryUpdate().Subscribe(_ => HandleSwipeState());
    }

    private void OnCarUnclick()
    {
        _car = null;
        _startPosition = Vector2.zero;

        _updateSubscription.Dispose();
    }

    private void HandleSwipeState()
    {
        Vector2 delta = _carClickHandler.GetCurrentPosition() - _startPosition;
        float distance = delta.magnitude;

        if (distance > SwipeDistance)
        {
            Vector3 worldDirection = Utils.ConvertScreenSwipeToWorldDirection(delta.normalized, _camera.transform);
            int specificDirection = Utils.CalculateSwipeDirectionRelativeToCar(worldDirection, _car.transform);

            if(specificDirection != 0)
            {
                HasSwipe?.Invoke(_car, specificDirection);
                OnCarUnclick();
            }
        }
    }
}