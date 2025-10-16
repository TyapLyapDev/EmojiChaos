using System;
using TMPro;
using UnityEngine;

public class GunVisual : MonoBehaviour
{
    private const string SmileBackgroundColorProperty = "_BackgroundColor";
    private const string MuzzleColorProperty = "_Color";

    [SerializeField] private Renderer _smile;
    [SerializeField] private Renderer _muzzle;
    [SerializeField] private TextMeshProUGUI _countText;

    private MaterialPropertyBlock _smilePropertyBlock;
    private MaterialPropertyBlock _muzzlePropertyBlock;
    private int _smileBackgroundColorShaderId;
    private int _muzzleColorShaderId;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

        if (_muzzle == null)
            throw new NullReferenceException($"{nameof(_muzzle)} не назначен");

        if (_smile == null)
            throw new NullReferenceException($"{nameof(_smile)} не назначен");

        _smilePropertyBlock = new();
        _muzzlePropertyBlock = new();
        _smileBackgroundColorShaderId = Shader.PropertyToID(SmileBackgroundColorProperty);
        _muzzleColorShaderId = Shader.PropertyToID(MuzzleColorProperty);

        _isInitialized = true;
    }

    public void SetColor(Color color)
    {
        ValidateInitialization(nameof(SetColor));

        _smile.GetPropertyBlock(_smilePropertyBlock);
        _smilePropertyBlock.SetColor(_smileBackgroundColorShaderId, color);
        _smile.SetPropertyBlock(_smilePropertyBlock);

        _muzzle.GetPropertyBlock(_muzzlePropertyBlock);
        _muzzlePropertyBlock.SetColor(_muzzleColorShaderId, color);
        _muzzle.SetPropertyBlock(_muzzlePropertyBlock);

        _countText.color = color;
    }

    public void SetBulletCount(int bulletCount)
    {
        ValidateInitialization(nameof(SetBulletCount));
        _countText.text = bulletCount.ToString();
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван перед инициализацией. Сначала вызовите {nameof(Initialize)}");
    }
}