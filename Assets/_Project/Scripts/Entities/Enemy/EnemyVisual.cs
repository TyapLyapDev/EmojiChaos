using UnityEngine;

public class EnemyVisual : InitializingBehaviour
{
    [SerializeField] private SimpleRepainter _repainter;
    [SerializeField] private Animator _animator;

    private EnemyAnimator _enemyAnimator;

    public void SetColor(Color color)
    {
        ValidateInit(nameof(SetColor));
        _repainter.SetColor(color);
    }

    protected override void OnInitialize()
    {
        _enemyAnimator = new(_animator);
        _repainter.Initialize();
    }
}