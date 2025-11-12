using System;

public class FactoryWithConfig<TBehaviour, TConfig> : BaseFactory<TBehaviour>
    where TBehaviour : InitializingWithConfigBehaviour<TConfig>
    where TConfig : IConfig
{
    private readonly TConfig _config;

    public FactoryWithConfig(TBehaviour prefab, TConfig config) : base(prefab)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    protected override void OnCreate(TBehaviour element) =>
        element.Initialize(_config);
}