using System;
using EmojiChaos.Core.Abstract.Interface;
using EmojiChaos.Services.Core;
using EmojiChaos.Services.Movement;

namespace EmojiChaos.Entities.Star
{
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