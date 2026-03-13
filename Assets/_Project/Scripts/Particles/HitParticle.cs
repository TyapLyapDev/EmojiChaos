using System;
using EmojiChaos.Core.Abstract;
using EmojiChaos.Core.Abstract.Interface;

namespace EmojiChaos.Particles
{
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