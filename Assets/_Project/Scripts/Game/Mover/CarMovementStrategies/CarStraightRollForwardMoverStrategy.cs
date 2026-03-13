using System;
using EmojiChaos.Core.Abstract;
using UnityEngine;

namespace EmojiChaos.Game.Mover.CarMovementStrategies
{
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