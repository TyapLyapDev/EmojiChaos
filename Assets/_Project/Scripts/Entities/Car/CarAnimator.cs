using System;
using UnityEngine;

public class CarAnimator : InitializingBehaviour
{
    private readonly int s_forwardAccidentId = Animator.StringToHash("IsForwardAccident");
    private readonly int s_backwardAccidentId = Animator.StringToHash("IsBackwardAccident");

    [SerializeField] private Animator _animator;

    private Action _forwardAccidentCompleted;
    private Action _backwardAccidentCompleted;

    public void SetForwardAccident(Action accidentCompleted)
    {
        _forwardAccidentCompleted = accidentCompleted;
        _animator.SetBool(s_forwardAccidentId, true);
    }

    public void SetBackwardAccident(Action accidentCompleted)
    {
        _backwardAccidentCompleted = accidentCompleted;
        _animator.SetBool(s_backwardAccidentId, true);
    }

    public void OnEndForwardAccident()
    {
        _animator.SetBool(s_forwardAccidentId, false);
        _forwardAccidentCompleted?.Invoke();
    }

    public void OnEndBackwardAccident()
    {
        _animator.SetBool(s_backwardAccidentId, false);
        _backwardAccidentCompleted?.Invoke();
    }

    protected override void OnInitialize() { }
}