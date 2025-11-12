public class Factory<TBehaviour> : BaseFactory<TBehaviour> where TBehaviour : InitializingBehaviour
{
    public Factory(TBehaviour prefab) : base(prefab) { }

    protected override void OnCreate(TBehaviour element) =>
        element.Initialize();
}