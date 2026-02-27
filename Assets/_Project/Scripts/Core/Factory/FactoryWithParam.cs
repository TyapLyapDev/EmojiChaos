using System;

public class FactoryWithParam<TBehaviour, TParam> : BaseFactory<TBehaviour>
    where TBehaviour : InitializingWithConfigBehaviour<TParam>
    where TParam : IParam
{
    private readonly TParam _config;

    public FactoryWithParam(TBehaviour prefab, TParam param) : base(prefab)
    {
        _config = param ?? throw new ArgumentNullException(nameof(param));
    }

    protected override void OnCreate(TBehaviour element) =>
        element.Initialize(_config);
}