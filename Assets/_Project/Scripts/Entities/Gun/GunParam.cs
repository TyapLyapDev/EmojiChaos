using System;
using EmojiChaos.Core.Abstract.Interface;
using EmojiChaos.Services.Combat;
using EmojiChaos.Services.Core;

namespace EmojiChaos.Entities.Guns
{
    public readonly struct GunParam : IParam
    {
        private readonly Shooter _shooter;
        private readonly ParticleShower _particleShower;
        private readonly float _timeReload;

        public GunParam(Shooter shooter, ParticleShower particleShower, float timeReload)
        {
            _shooter = shooter ?? throw new ArgumentNullException(nameof(shooter));
            _particleShower = particleShower ?? throw new ArgumentNullException(nameof(particleShower));

            if (timeReload <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeReload), timeReload, "The value must be greater than zero");

            _timeReload = timeReload;
        }

        public Shooter Shooter => _shooter;

        public ParticleShower ParticleShower => _particleShower;

        public float TimeReload => _timeReload;
    }
}