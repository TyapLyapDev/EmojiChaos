using System;
using UnityEngine;

public class Aimer
{
    private readonly Transform _rotatingModel;

    public Aimer(Transform rotatingModel)
    {
        _rotatingModel = rotatingModel ?? throw new ArgumentNullException(nameof(rotatingModel));
    }

    public void AimAtTarget(Transform target)
    {
        if (target != null && target != _rotatingModel)
            _rotatingModel.LookAt(target);
    }

    public void ResetRotation() =>
        _rotatingModel.rotation = Quaternion.identity;
}