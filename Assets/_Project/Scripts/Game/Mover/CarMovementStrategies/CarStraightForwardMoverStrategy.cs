using System;
using UnityEngine;

public class CarStraightForwardMoverStrategy : CarStraightMoverBaseStrategy
{
    public CarStraightForwardMoverStrategy(Transform transform,
        BoxCollider self,
        Action<Vector3> obstacleCollision,
        Action<CarSplineContainer, Vector3> roadDetected) : base(transform, self, obstacleCollision, roadDetected) { }

    protected override Vector3 GetDirection() =>
        Transform.forward;
}