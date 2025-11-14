using System;
using UnityEngine;

public class EnemyVisual : InitializingBehaviour
{
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private SimpleRepainter _repainter;
    [SerializeField] private AlphaMaskTextureReplacer _replacer;
    [SerializeField] private Texture2D[] _deathTextures;
    [SerializeField] private Texture2D[] _fearTextures;
    [SerializeField] private Texture2D[] _angryTextures;

    public event Action DiedCompleted;

    private void OnDestroy()
    {
        _animator.DiedComleted -= OnDiedCompleted;
    }

    public void SetColor(Color color)
    {
        ValidateInit(nameof(SetColor));
        _repainter.SetColor(color);
    }

    public void SetDied()
    {
        _animator.SetDied();
        SetRandom(_deathTextures);
    }

    public void ResetDied()
    {
        _animator.ResetDied();
        SetRandom(_angryTextures);
    }

    public void SetFear()
    {
        SetRandom(_fearTextures);
    }

    protected override void OnInitialize()
    {
        _repainter.Initialize();
        _replacer.Initialize();
        _animator.Initialize();

        _animator.DiedComleted += OnDiedCompleted;
    }

    private void OnDiedCompleted() =>
        DiedCompleted?.Invoke();

    private void SetRandom(Texture2D[] textures) =>
        _replacer.SetTexture(textures[UnityEngine.Random.Range(0, textures.Length)]);
}