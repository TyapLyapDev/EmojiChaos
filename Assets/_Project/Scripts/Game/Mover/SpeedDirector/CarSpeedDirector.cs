using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CarSpeedDirector : IDisposable
{
    private const float Speed = 4f;

    private readonly List<Car> _cars = new();
    private IDisposable _updateSubscription;

    public void Dispose()
    {
        _updateSubscription?.Dispose();
        _cars.Clear();
    }

    public void Register(Car car)
    {
        _cars.Add(car);

        if (_cars.Count == 1)
            _updateSubscription = Observable.EveryUpdate().Subscribe(_ => ProcessMovement());
    }

    public void Unregister(Car car)
    {
        _cars.Remove(car);

        if (_cars.Count == 0)
        {
            _updateSubscription?.Dispose();
            _updateSubscription = null;
        }
    }

    private void ProcessMovement()
    {
        float deltaDistance = Speed * Time.deltaTime;

        for (int i = _cars.Count - 1; i >= 0; i--)
        {
            Car car = _cars[i];

            if (car != null)
                car.Move(deltaDistance);
            else
                Unregister(car);
        }            
    }
}