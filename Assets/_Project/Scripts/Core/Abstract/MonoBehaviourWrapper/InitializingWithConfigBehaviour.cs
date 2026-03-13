using EmojiChaos.Core.Abstract.Interface;

namespace EmojiChaos.Core.Abstract.MonoBehaviourWrapper
{
    public abstract class InitializingWithConfigBehaviour<TConfig> : BaseInitializingBehaviour
        where TConfig : IParam
    {
        public void Initialize(TConfig config)
        {
            BaseInitialize();
            OnInitialize(config);
        }

        protected abstract void OnInitialize(TConfig config);
    }
}