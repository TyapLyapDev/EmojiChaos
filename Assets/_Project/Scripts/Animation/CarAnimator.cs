using System;
using UnityEngine;

public class CarAnimator : InitializingBehaviour
{
    private static readonly int s_forwardAccidentId = Animator.StringToHash("CarForwardAccident");
    private static readonly int s_backwardAccidentId = Animator.StringToHash("CarBackwardAccident");
    private static readonly int s_unavailableId = Animator.StringToHash("CarUnavailable");

    [SerializeField] private Animator _animator;

    private Action _forwardAccidentCompleted;
    private Action _backwardAccidentCompleted;

    public void PlayForwardAccident(Action accidentCompleted)
    {
        _forwardAccidentCompleted = accidentCompleted;
        _animator.Play(s_forwardAccidentId, -1, 0f);
    }

    public void PlayBackwardAccident(Action accidentCompleted)
    {
        _backwardAccidentCompleted = accidentCompleted;
        _animator.Play(s_backwardAccidentId, -1, 0f);
    }

    public void PlayUnavailable() =>
        _animator.Play(s_unavailableId, -1, 0f);

    protected override void OnInitialize() { }

    private void OnEndForwardAccident() =>
        _forwardAccidentCompleted?.Invoke();

    private void OnEndBackwardAccident() =>
        _backwardAccidentCompleted?.Invoke();
}