using EmojiChaos.Core.Abstract;
using EmojiChaos.Core.Abstract.MonoBehaviourWrapper;

namespace EmojiChaos.Core.Factory
{
    public class Factory<TBehaviour> : BaseFactory<TBehaviour>
        where TBehaviour : InitializingBehaviour
    {
        public Factory(TBehaviour prefab)
            : base(prefab) { }

        protected override void OnCreate(TBehaviour element) =>
            element.Initialize();
    }
}