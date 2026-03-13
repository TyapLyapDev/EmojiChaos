using System;
using EmojiChaos.Core.Abstract.Interface;
using EmojiChaos.Entities.Cars;
using EmojiChaos.Entities.Racks;
using EmojiChaos.Services.Input;
using EmojiChaos.Services.Movement;
using EmojiChaos.Services.Spawning.EnemySpawner;
using UnityEngine;

namespace EmojiChaos.UI.TutorialSpace
{
    public readonly struct TutorialParam : IParam
    {
        private readonly Transform _portalTarget;
        private readonly EnemySpawner _enemySpawner;
        private readonly ISwipeStrategy _swipeStrategy;
        private readonly Car _car;
        private readonly EnemiesMovementDirector _enemiesSpeedDirector;
        private readonly Rack[] _racks;
        private readonly Entities.Star.Star _star;

        public TutorialParam(
            Transform portalTarget,
            EnemySpawner enemySpawner,
            Car car,
            ISwipeStrategy swipeStrategy,
            EnemiesMovementDirector enemiesSpeedDirector,
            Rack[] racks,
            Entities.Star.Star star)
        {
            _portalTarget = portalTarget != null ? portalTarget : throw new ArgumentNullException(nameof(portalTarget));
            _enemySpawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
            _car = car != null ? car : throw new ArgumentNullException(nameof(car));
            _swipeStrategy = swipeStrategy ?? throw new ArgumentNullException(nameof(swipeStrategy));
            _enemiesSpeedDirector = enemiesSpeedDirector ?? throw new ArgumentNullException(nameof(enemiesSpeedDirector));
            _racks = racks ?? throw new ArgumentNullException(nameof(racks));
            _star = star != null ? star : throw new ArgumentNullException(nameof(star));
        }

        public Transform PortalTarget => _portalTarget;

        public EnemySpawner EnemySpawner => _enemySpawner;

        public Car Car => _car;

        public ISwipeStrategy SwipeStrategy => _swipeStrategy;

        public EnemiesMovementDirector EnemiesSpeedDirector => _enemiesSpeedDirector;

        public Rack[] Racks => _racks;

        public Entities.Star.Star Star => _star;
    }
}