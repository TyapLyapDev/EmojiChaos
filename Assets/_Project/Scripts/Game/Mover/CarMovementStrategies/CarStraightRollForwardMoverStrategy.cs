using System;
using UnityEngine;

namespace EmojiChaos.Game.Mover.CarMovementStrategies
{
    using Core.Abstract;

    public class CarStraightRollForwardMoverStrategy : CarStraightRollMoverBaseStrategy
    {
        public CarStraightRollForwardMoverStrategy(
            Transform transform,
            BoxCollider self,
            Action completed)
            : base(transform, self, completed) { }

        protected override Vector3 GetDirection() =>
            Transform.forward;
    }
}