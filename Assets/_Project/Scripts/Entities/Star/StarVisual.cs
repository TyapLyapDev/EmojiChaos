using System;
using UnityEngine;

public class StarVisual : InitializingBehaviour
{
    [SerializeField] private StarAnimator _animator;
    [SerializeField] private AlphaMaskTextureReplacer _replacer;
    [SerializeField] private Texture2D[] _deathTextures;
    [SerializeField] private Texture2D[] _fearTextures;
    [SerializeField] private Texture2D[] _enjoyTextures;

    public event Action DiedCompleted;

    private void OnDestroy()
    {
        _animator.DiedComleted -= OnDiedCompleted;
    }

    public void SetEnjoy()
    {
        _animator.SetEnjoy();
        SetRandom(_enjoyTextures);
    }

    public void SetFear()
    {
        _animator.SetFear();
        SetRandom(_fearTextures);
    }

    public void SetDied()
    {
        _animator.SetDied();
        SetRandom(_deathTextures);
    }  

    protected override void OnInitialize()
    {
        _replacer.Initialize();
        _animator.Initialize();

        _animator.DiedComleted += OnDiedCompleted;
    }

    private void OnDiedCompleted() =>
        DiedCompleted?.Invoke();

    private void SetRandom(Texture2D[] textures) =>
        _replacer.SetTexture(textures[UnityEngine.Random.Range(0, textures.Length)]);
}