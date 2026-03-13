using System;
using EmojiChaos.Core.Abstract.Interface;
using EmojiChaos.Services.Core;
using UnityEngine.Splines;

namespace EmojiChaos.Entities.Enemy
{
    public readonly struct EnemyParam : IParam
    {
        private readonly SplineContainer _splineContainer;
        private readonly ParticleShower _particleShower;

        public EnemyParam(SplineContainer splineContainer, ParticleShower particleShower)
        {
            _splineContainer = splineContainer != null ? splineContainer : throw new ArgumentNullException(nameof(splineContainer));
            _particleShower = particleShower ?? throw new ArgumentNullException(nameof(particleShower));
        }

        public SplineContainer SplineContainer => _splineContainer;

        public ParticleShower ParticleShower => _particleShower;
    }
}