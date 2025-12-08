using System;
using UnityEngine;

public class EnemyAnimator : InitializingBehaviour
{
    private static readonly int s_hashIsDied = Animator.StringToHash("IsDied");

    [SerializeField] private Animator _animator;

    public event Action DiedComleted;

    public void PlayIdle()
    {
        ValidateInit(nameof(PlayIdle));
        _animator.SetBool(s_hashIsDied, false);
    }

    public void PlayDied()
    {
        ValidateInit(nameof(PlayDied));
        _animator.SetBool(s_hashIsDied, true);
    }

    protected override void OnInitialize() { }

    private void OnDiedComleted() =>
        DiedComleted?.Invoke();
}