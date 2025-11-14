using UnityEngine;

public abstract class BaseRepainter : InitializingBehaviour
{
    [SerializeField] private Renderer _renderer;

    private MaterialPropertyBlock _propertyBlock;

    protected void Repaint()
    {
        ValidateInit(nameof(Repaint));

        OnGetPropertyBlock(_renderer, _propertyBlock);
        OnRepaint(_propertyBlock);
        OnSetPropertyBlock(_renderer, _propertyBlock);
    }

    protected virtual void OnGetPropertyBlock(Renderer renderer, MaterialPropertyBlock propertyBlock) =>
        renderer.GetPropertyBlock(propertyBlock);

    protected abstract void OnRepaint(MaterialPropertyBlock propertyBlock);

    protected virtual void OnSetPropertyBlock(Renderer renderer, MaterialPropertyBlock propertyBlock) =>
        renderer.SetPropertyBlock(propertyBlock);

    protected override void OnInitialize() =>
        _propertyBlock = new();
}