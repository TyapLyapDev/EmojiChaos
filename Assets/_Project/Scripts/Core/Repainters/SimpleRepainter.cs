using System;
using UnityEngine;

public class SimpleRepainter : BaseRepainter
{
    [SerializeField] private string _propertyColor = "_BackgroundColor";

    private Color _color;
    private int _shaderId;

    public void SetColor(Color color)
    {
        _color = color;
        Repaint();
    }

    protected override void OnRepaint(MaterialPropertyBlock propertyBlock) =>
        propertyBlock.SetColor(_shaderId, _color);

    protected override void OnInitialize()
    {
        base.OnInitialize();

        if (string.IsNullOrEmpty(_propertyColor))
            throw new InvalidOperationException(nameof(_propertyColor));

        _shaderId = Shader.PropertyToID(_propertyColor);
    }
}