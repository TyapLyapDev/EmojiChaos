using System;
using UnityEngine;

public interface IHittable : IRepaintable
{
    event Action<IHittable> Disappeared;

    Transform CenterBody { get; }

    bool IsActive { get; }

    void Kill();
}