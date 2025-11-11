using UnityEngine;

public class CarVisual : InitializingBehaviour
{
    [SerializeField] private MaterialIndexRepainter _repainter;

    public void SetColor(Color color)
    {
        ValidateInit(nameof(SetColor));

        _repainter.SetColor(color);
    }

    protected override void OnInitialize() =>
        _repainter.Initialize();
}