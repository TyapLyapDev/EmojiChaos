using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CarService : IDisposable
{
    private readonly List<Car> _cars;
    private readonly TypeColorRandomizer _colorRandomizer;
    private readonly CarSpeedDirector _speedDirector = new();
    private readonly AttackSystem _attackSystem;
    private readonly MapSplineNodes _mapSplineNodes;
    private readonly ISwipeStrategy _swipeHandler;

    public CarService(List<Car> cars, TypeColorRandomizer colorRandomizer, AttackSystem attackSystem, SplineContainer carRoadSpline)
    {
        _cars = cars;
        _colorRandomizer = colorRandomizer;
        _attackSystem = attackSystem;
        _mapSplineNodes = new(carRoadSpline);
        _swipeHandler = new CarSwipeStrategy();

        InitializeCars();

        _swipeHandler.HasSwipe += OnSwipe;
    }

    public void Dispose()
    {
        _swipeHandler.HasSwipe -= OnSwipe;

        if(_swipeHandler is IDisposable disposable)
            disposable.Dispose();

        _speedDirector?.Dispose();
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

    private void OnSwipe(ISwipeable swipeableObject, int direction)
    {
        if (swipeableObject is Car car)
            if (_attackSystem.TryGetAvailableSlot(out AttackSlot attackSlot))
                if (car.TryReservationSlot(attackSlot, direction))
                    _speedDirector.Register(car);
    }
}