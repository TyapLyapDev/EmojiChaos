using System;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private const string BackgroundColorProperty = "_BackgroundColor";

    [SerializeField] private Renderer _renderer;
    [SerializeField] private Animator _animator;

    private EnemyAnimator _customAnimator;
    private MaterialPropertyBlock _propertyBlock;
    private int _backgroundColorShaderId;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

        if (_renderer == null)
            throw new NullReferenceException(nameof(_renderer));

        if (_animator == null)
            throw new NullReferenceException(nameof(_animator));

        _customAnimator = new(_animator);
        _propertyBlock = new();
        _backgroundColorShaderId = Shader.PropertyToID(BackgroundColorProperty);

        _isInitialized = true;
    }

    public void SetColor(Color color)
    {
        ValidateInitialization(nameof(SetColor));

        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(_backgroundColorShaderId, color);
        _renderer.SetPropertyBlock(_propertyBlock);
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван перед инициализацией. Сначала вызовите {nameof(Initialize)}");
    }
}