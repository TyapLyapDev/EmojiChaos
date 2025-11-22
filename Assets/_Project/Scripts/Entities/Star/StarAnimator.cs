using System;
using UnityEngine;

public class StarAnimator : InitializingBehaviour
{
    private static readonly int s_hashIsFear = Animator.StringToHash("IsFear");
    private static readonly int s_hashIsDied = Animator.StringToHash("IsDied");

    [SerializeField] private Animator _animator;

    public event Action DiedComleted;

    public void SetEnjoy() =>
        _animator.SetBool(s_hashIsFear, false);

    public void SetFear() =>
        _animator.SetBool(s_hashIsFear, true);

    public void SetDied() =>
        _animator.SetBool(s_hashIsDied, true);

    protected override void OnInitialize() { }

    private void OnDiedComleted() =>
        DiedComleted?.Invoke();
}