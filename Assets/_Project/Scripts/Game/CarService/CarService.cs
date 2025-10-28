using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CarService : IDisposable
{
    private readonly List<Car> _cars;
    private readonly TypeColorRandomizer _colorRandomizer;
    private readonly CarSwipeHandler _swipeHandler;
    private readonly CarSpeedDirector _speedDirector = new();
    private readonly AttackSystem _attackSystem;
    private readonly MapSplineNodes _mapSplineNodes;

    public CarService(List<Car> cars, TypeColorRandomizer colorRandomizer, AttackSystem attackSystem, SplineContainer carRoadSpline)
    {
        _cars = cars;
        _colorRandomizer = colorRandomizer;
        _swipeHandler = new();
        _attackSystem = attackSystem;
        _mapSplineNodes = new(carRoadSpline);

        InitializeCars();

        _swipeHandler.HasSwipe += OnSwipe;
    }

    public void Dispose()
    {
        _swipeHandler.HasSwipe -= OnSwipe;
        _swipeHandler.Dispose();
        _speedDirector.Dispose();
    }

    private void InitializeCars()
    {
        List<Car> cars = new(_cars);

        foreach (Car car in cars)
        {
            if (car != null)
            {
                car.Initialize(_mapSplineNodes);

                if (_colorRandomizer.TryGetColor(car.Id, out Color color))
                    car.SetColor(color);
            }
        }
    }

    private void OnSwipe(Car car, int direction)
    {
        if (_attackSystem.TryGetAvailableSlot(out AttackSlot attackSlot))
            if (car.TryReservationSlot(attackSlot, direction))
                _speedDirector.Register(car);
    }
}