using UnityEngine;

public class EnemyAnimator
{
    private const string IsWalk = nameof(IsWalk);

    private readonly Animator _animator;
    private readonly int _hashIsWalk;

    public EnemyAnimator(Animator animator)
    {
        _animator = animator;
        _hashIsWalk = Animator.StringToHash(IsWalk);
    }

    public void SetWalk() =>
        _animator.SetBool(_hashIsWalk, true);

    public void SetIdle() =>
        _animator.SetBool(_hashIsWalk, false);
}