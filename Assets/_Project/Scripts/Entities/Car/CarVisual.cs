using System;
using UnityEngine;

public class CarVisual : InitializingBehaviour
{
    [SerializeField] private MaterialIndexRepainter _repainter;
    [SerializeField] private CarAnimator _carAnimator;

    public void SetColor(Color color)
    {
        ValidateInit(nameof(SetColor));

        _repainter.SetColor(color);
    }

    public void SetForwardAccident(Action accidentCompleted) =>
        _carAnimator.SetForwardAccident(accidentCompleted);

    public void SetBackwardAccident(Action accidentCompleted) =>
        _carAnimator.SetBackwardAccident(accidentCompleted);

    protected override void OnInitialize() =>
        _repainter.Initialize();
}