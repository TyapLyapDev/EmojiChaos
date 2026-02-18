using UnityEngine;

public class GunAnimator : InitializingBehaviour
{
    private static readonly int s_hashDisappearance = Animator.StringToHash("GunDisappearance");
    private static readonly int s_hashShoot = Animator.StringToHash("GunShot");
    private static readonly int s_hashShootAnimation = Animator.StringToHash("GunShot");

    [SerializeField] private Animator _shooter;
    [SerializeField] private Animator _gun;

    public void PlayHidding()
    {
        ValidateInit(nameof(PlayHidding));
        _shooter.Play(s_hashDisappearance, -1, 0f);
    }

    public void PlayShooting()
    {
        ValidateInit(nameof(PlayShooting));
        _shooter.Play(s_hashShoot, -1, 0f);
        _gun.Play(s_hashShootAnimation, -1, 0f);
    }

    protected override void OnInitialize() { }
}