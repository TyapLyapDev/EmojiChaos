using System;
using UnityEngine;

namespace EmojiChaos.Game.Mover.CarMovementStrategies
{
    using Core.Abstract;
    using Entities.Markers;

    public class CarStraightBackwardMoverStrategy : CarStraightMoverBaseStrategy
    {
        public CarStraightBackwardMoverStrategy(
            Transform transform,
            BoxCollider self,
            Action<Vector3> obstacleCollision,
            Action<CarSplineContainer, Vector3> roadDetected)
            : base(transform, self, obstacleCollision, roadDetected) { }

        protected override Vector3 GetDirection() =>
            -Transform.forward;
    }
}