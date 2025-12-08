using TMPro;
using UnityEngine;

public class GunVisual : InitializingBehaviour
{
    [SerializeField] private SimpleRepainter[] _repainters;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private GunAnimator _animator;

    public void SetColor(Color color)
    {
        ValidateInit(nameof(SetColor));

        foreach (SimpleRepainter repainter in _repainters)
            repainter.SetColor(color);

        _countText.color = color;
    }

    public void DisplayBulletCount(int bulletCount)
    {
        ValidateInit(nameof(DisplayBulletCount));
        _countText.text = bulletCount.ToString();
    }

    public void SetHidding()
    {
        _animator.PlayHidding();
        _countText.text = string.Empty;
    }

    public void SetShooting() =>
        _animator.PlayShooting();

    protected override void OnInitialize()
    {
        foreach (SimpleRepainter repainter in _repainters)
            repainter.Initialize();

        _animator.Initialize();
    }
}