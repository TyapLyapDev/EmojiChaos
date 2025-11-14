using System;
using UnityEngine;

public class CarVisual : InitializingBehaviour
{
    [SerializeField] private MaterialIndexRepainter _repainter;
    [SerializeField] private CarAnimator _carAnimator;

    public event Action ForwardAccidentCompleted;
    public event Action BackwardAccidentCompleted;

    private void OnDestroy()
    {
        if (_carAnimator != null)
        {
            _carAnimator.ForwardAccidentCompleted -= OnForwardAccidentCompleted;
            _carAnimator.BackwardAccidentCompleted -= OnBackwardAccidentCompleted;
        }
    }

    public void SetColor(Color color)
    {
        ValidateInit(nameof(SetColor));

        _repainter.SetColor(color);
    }

    public void SetForwardAccident() =>
        _carAnimator.SetForwardAccident();

    public void SetBackwardAccident() =>
        _carAnimator.SetBackwardAccident();

    protected override void OnInitialize()
    {
        _repainter.Initialize();

        _carAnimator.ForwardAccidentCompleted += OnForwardAccidentCompleted;
        _carAnimator.BackwardAccidentCompleted += OnBackwardAccidentCompleted;
    }

    private void OnForwardAccidentCompleted() =>
        ForwardAccidentCompleted?.Invoke();

    private void OnBackwardAccidentCompleted() =>
        BackwardAccidentCompleted?.Invoke();
}