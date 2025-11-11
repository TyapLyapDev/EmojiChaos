using UnityEngine;

public class BulletVisual : InitializingBehaviour
{
    [SerializeField] private SimpleRepainter[] _repainters;

    public void SetColor(Color color)
    {
        ValidateInit(nameof(SetColor));

        foreach (SimpleRepainter repainter in _repainters)
            repainter.SetColor(color);
    }

    protected override void OnInitialize()
    {
        foreach (SimpleRepainter repainter in _repainters)
            repainter.Initialize();
    }
}