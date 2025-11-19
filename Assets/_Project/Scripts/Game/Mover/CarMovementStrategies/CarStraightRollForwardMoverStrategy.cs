using System;
using UnityEngine;

public class CarStraightRollForwardMoverStrategy : CarStraightRollMoverBaseStrategy
{
    public CarStraightRollForwardMoverStrategy(Transform transform,
        BoxCollider self,
        Action completed) : base(transform, self, completed) { }

    protected override Vector3 GetDirection() => 
        Transform.forward;
}