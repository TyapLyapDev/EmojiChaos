public abstract class InitializingWithConfigBehaviour<TConfig> : BaseInitializingBehaviour
    where TConfig : IConfig
{
    public void Initialize(TConfig config)
    {
        BaseInitialize();
        OnInitialize(config);
    }

    protected abstract void OnInitialize(TConfig config);
}