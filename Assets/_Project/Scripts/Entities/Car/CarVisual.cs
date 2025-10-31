using System;
using UnityEngine;

public class CarVisual : MonoBehaviour
{
    private const string ColorProperty = "_Color";

    [SerializeField] private Renderer _renderer;
    [SerializeField] private int _targetMaterialIndex = 0;

    private MaterialPropertyBlock _propertyBlock;
    private int _colorShaderId;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

        if (_renderer == null)
            throw new Exception($"{nameof(_renderer)} не назначен");

        _propertyBlock = new();
        _colorShaderId = Shader.PropertyToID(ColorProperty);

        _isInitialized = true;
    }

    public void SetColor(Color color)
    {
        ValidateInitialization(nameof(SetColor));

        _renderer.GetPropertyBlock(_propertyBlock, _targetMaterialIndex);
        _propertyBlock.SetColor(_colorShaderId, color);
        _renderer.SetPropertyBlock(_propertyBlock, _targetMaterialIndex);
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван перед инициализацией. Сначала вызовите {nameof(Initialize)}");
    }
}