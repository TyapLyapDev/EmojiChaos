using TMPro;
using UnityEngine;

public class GunVisual : MonoBehaviour
{
    private const string SmileBackgroundColorName = "_BackgroundColor";
    private const string MuzzleColorName = "_Color";

    [SerializeField] private Renderer _smile;
    [SerializeField] private Renderer _muzzle;
    [SerializeField] private TextMeshProUGUI _countText;

    private MaterialPropertyBlock _smilePropertyBlock;
    private MaterialPropertyBlock _muzzlePropertyBlock;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_muzzle == null)
            throw new System.Exception($"{nameof(_muzzle)} не назначен");

        if (_smile == null)
            throw new System.Exception($"{nameof(_smile)} не назначен");

        _smilePropertyBlock = new();
        _muzzlePropertyBlock = new();
        _isInitialized = true;
    }

    public void SetColor(Color color)
    {
        if (_isInitialized == false)
            throw new System.Exception("Попытка назначить цвет до инициализации");

        _smile.GetPropertyBlock(_smilePropertyBlock);
        _smilePropertyBlock.SetColor(SmileBackgroundColorName, color);
        _smile.SetPropertyBlock(_smilePropertyBlock);

        _muzzle.GetPropertyBlock(_muzzlePropertyBlock);
        _muzzlePropertyBlock.SetColor(MuzzleColorName, color);
        _muzzle.SetPropertyBlock(_muzzlePropertyBlock);

        _countText.color = color;
    }

    public void SetText(int bulletCount) =>
        _countText.text = bulletCount.ToString();
}