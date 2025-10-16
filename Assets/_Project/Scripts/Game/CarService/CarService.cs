using System;
using System.Collections.Generic;
using UnityEngine;

public class CarService : IDisposable
{
    private readonly List<Car> _cars;
    private readonly TypeColorRandomizer _colorRandomizer;

    public event Action<Car> CarClicked;

    public CarService(List<Car> cars, TypeColorRandomizer colorRandomizer)
    {
        _cars = cars;
        _colorRandomizer = colorRandomizer;

        foreach (Car car in _cars)
        {
            if (car != null)
            {
                car.Initialize();

                if (_colorRandomizer.TryGetColor(car.Id, out Color color))
                    car.SetColor(color);

                car.Clicked += OnCarClciked;
            }
        }
    }

    public void Dispose()
    {
        List<Car> cars = new(_cars);

        foreach (Car car in cars)
            if (car != null)
                car.Clicked -= OnCarClciked;
    }

    private void OnCarClciked(Car car) =>
        CarClicked?.Invoke(car);
}