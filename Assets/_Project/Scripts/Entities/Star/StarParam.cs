using System;

namespace EmojiChaos.Entities.Star
{
    using Core.Abstract.Interface;
    using Services.Core;
    using Services.Movement;

    public readonly struct StarParam : IParam
    {
        private readonly EnemiesMovementDirector _enemySpeedDirector;
        private readonly ParticleShower _particleShower;
        private readonly CameraShaker _cameraShaker;

        public StarParam(EnemiesMovementDirector enemySpeedDirector, ParticleShower particleShower, CameraShaker cameraShaker)
        {
            _enemySpeedDirector = enemySpeedDirector != null ? enemySpeedDirector : throw new ArgumentNullException(nameof(enemySpeedDirector));
            _particleShower = particleShower ?? throw new ArgumentNullException(nameof(particleShower));
            _cameraShaker = cameraShaker ?? throw new ArgumentNullException(nameof(cameraShaker));
        }

        public EnemiesMovementDirector EnemySpeedDirector => _enemySpeedDirector;

        public ParticleShower ParticleShower => _particleShower;

        public CameraShaker CameraShaker => _cameraShaker;
    }
}