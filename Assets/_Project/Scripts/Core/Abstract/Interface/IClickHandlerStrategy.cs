using System;
using UnityEngine;

public interface IClickHandlerStrategy
{
    event Action<IClickable, Vector2> Clicked;
    event Action Unclicked;

    Vector2 GetCurrentPosition();
}