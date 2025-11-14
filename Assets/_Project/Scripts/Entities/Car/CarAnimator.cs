using System;
using UnityEngine;

public class CarAnimator : InitializingBehaviour
{
    private readonly int s_forwardAccidentId = Animator.StringToHash("IsForwardAccident");
    private readonly int s_backwardAccidentId = Animator.StringToHash("IsBackwardAccident");

    [SerializeField] private Animator _animator;

    public event Action ForwardAccidentCompleted;
    public event Action BackwardAccidentCompleted;

    public void SetForwardAccident() =>
        _animator.SetBool(s_forwardAccidentId, true);

    public void SetBackwardAccident() =>
        _animator.SetBool(s_backwardAccidentId, true);

    public void OnEndForwardAccident()
    {
        _animator.SetBool(s_forwardAccidentId, false);
        ForwardAccidentCompleted?.Invoke();
    }

    public void OnEndBackwardAccident()
    {
        _animator.SetBool(s_backwardAccidentId, false);
        BackwardAccidentCompleted?.Invoke();
    }

    protected override void OnInitialize() { }
}