using System;

namespace EmojiChaos.Particles
{
    using Core.Abstract;
    using Core.Abstract.Interface;

    public class HitParticle : OneShotParticle, IPoolable<HitParticle>
    {
        public event Action<HitParticle> Deactivated;

        public void Deactivate()
        {
            ValidateInit(nameof(Deactivate));

            gameObject.SetActive(false);
            Deactivated?.Invoke(this);
        }

        protected override void OnCompleted() =>
            Deactivate();
    }
}