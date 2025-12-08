using UnityEngine;

public class GunAnimator : InitializingBehaviour
{
    private static readonly int s_hashDisappearance = Animator.StringToHash("GunDisappearance");
    private static readonly int s_hashShoot = Animator.StringToHash("GunShot");

    [SerializeField] private Animator _animator;

    public void PlayHidding()
    {
        ValidateInit(nameof(PlayHidding));
        _animator.Play(s_hashDisappearance, -1, 0f);
    }

    public void PlayShooting()
    {
        ValidateInit(nameof(PlayShooting));
        _animator.Play(s_hashShoot, -1, 0f);
    }

    protected override void OnInitialize() { }
}