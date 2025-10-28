using System;
using UnityEngine;

public interface ICarClickHandlerStrategy : IDisposable
{
    event Action<Car, Vector2> Clicked;
    event Action Unclicked;

    Vector2 GetCurrentPosition();
}
