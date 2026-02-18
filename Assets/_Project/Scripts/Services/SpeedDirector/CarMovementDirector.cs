using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CarMovementDirector : IDisposable
{
    private readonly float _speed = 5f;

    private readonly List<Car> _cars = new();
    private IDisposable _updateSubscription;

    public CarMovementDirector(float speed)
    {
        if(speed < 0)
            throw new ArgumentOutOfRangeException(nameof(speed), speed, "Значение должно быть положительным");

        _speed = speed;
    }

    public void Dispose()
    {
        _updateSubscription?.Dispose();
        _cars.Clear();
    }

    public void Register(Car car)
    {
        if (_cars.Contains(car))
            return;

        _cars.Add(car);
        car.EnableSmoke();

        if (_cars.Count == 1)
            _updateSubscription = Observable.EveryUpdate().Subscribe(_ => ProcessMovement());
    }

    public void Unregister(Car car)
    {
        _cars.Remove(car);
        car.DisableSmoke();

        if (_cars.Count == 0)
        {
            _updateSubscription?.Dispose();
            _updateSubscription = null;
        }
    }

    private void ProcessMovement()
    {
        float deltaDistance = _speed * Time.deltaTime;

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