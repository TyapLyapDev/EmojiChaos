using System;
using UnityEngine;

public class SimpleRepainter : InitializingBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private string _propertyColor;

    private MaterialPropertyBlock _propertyBlock;
    private int _shaderId;

    public void SetColor(Color color)
    {
        ValidateInit(nameof(SetColor));

        OnGetPropertyBlock(_renderer, _propertyBlock);
        _propertyBlock.SetColor(_shaderId, color);
        OnSetPropertyBlock(_renderer, _propertyBlock);
    }

    protected virtual void OnGetPropertyBlock(Renderer renderer, MaterialPropertyBlock propertyBlock) =>
        renderer.GetPropertyBlock(propertyBlock);

    protected virtual void OnSetPropertyBlock(Renderer renderer, MaterialPropertyBlock propertyBlock) =>
        renderer.SetPropertyBlock(propertyBlock);

    protected override void OnInitialize()
    {
        if (_renderer == null)
            throw new NullReferenceException(nameof(_renderer));

        if (string.IsNullOrEmpty(_propertyColor))
            throw new InvalidOperationException(nameof(_propertyColor));

        _propertyBlock = new();
        _shaderId = Shader.PropertyToID(_propertyColor);
    }
}