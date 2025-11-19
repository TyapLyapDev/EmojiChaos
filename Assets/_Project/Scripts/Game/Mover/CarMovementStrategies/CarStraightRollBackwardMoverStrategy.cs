using System;
using UnityEngine;

public class CarStraightRollBackwardMoverStrategy : CarStraightRollMoverBaseStrategy
{
    public CarStraightRollBackwardMoverStrategy(Transform transform, 
        BoxCollider self, 
        Action completed) : base(transform, self, completed) { }

    protected override Vector3 GetDirection() => 
        -Transform.forward;
}