using System;
using UnityEngine;

public class CarVisual : MonoBehaviour
{
    private const string ColorName = "_Color";

    [SerializeField] private Renderer _renderer;

    private MaterialPropertyBlock _propertyBlock;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_renderer == null)
            throw new Exception($"{nameof(_renderer)} �� ��������");

        _propertyBlock = new();
        _isInitialized = true;
    }

    public void SetColor(Color color)
    {
        if (_isInitialized == false)
            throw new Exception("������� ��������� ���� �� �������������");

        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(ColorName, color);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}