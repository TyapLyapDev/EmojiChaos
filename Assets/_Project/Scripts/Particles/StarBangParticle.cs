using System;
using EmojiChaos.Core.Abstract;
using EmojiChaos.Core.Abstract.Interface;

namespace EmojiChaos.Particles
{
    public class StarBangParticle : OneShotParticle, IPoolable<StarBangParticle>
    {
        public event Action<StarBangParticle> Deactivated;

        public void Deactivate()
        {
            gameObject.SetActive(false);
            Deactivated?.Invoke(this);
        }

        protected override void OnCompleted() =>
            Deactivate();
    }
}