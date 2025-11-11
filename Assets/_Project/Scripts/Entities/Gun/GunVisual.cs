using TMPro;
using UnityEngine;

public class GunVisual : InitializingBehaviour
{
    [SerializeField] private SimpleRepainter[] _repainters;
    [SerializeField] private TextMeshProUGUI _countText;

    private Color _color;

    public Color Color => _color;

    public void SetColor(Color color)
    {
        ValidateInit(nameof(SetColor));
        _color = color;

        foreach (SimpleRepainter repainter in _repainters)
            repainter.SetColor(color);

        _countText.color = color;
    }

    public void DisplayBulletCount(int bulletCount)
    {
        ValidateInit(nameof(DisplayBulletCount));
        _countText.text = bulletCount.ToString();
    }

    protected override void OnInitialize()
    {
        foreach (SimpleRepainter repainter in _repainters)
            repainter.Initialize();
    }
}