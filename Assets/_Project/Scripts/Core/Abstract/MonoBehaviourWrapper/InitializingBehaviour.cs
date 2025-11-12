public abstract class InitializingBehaviour : BaseInitializingBehaviour
{
    public void Initialize()
    {
        BaseInitialize();
        OnInitialize();
    }

    protected abstract void OnInitialize();
}