using System.Collections;
using UnityEngine;

public abstract class OneShotParticle : InitializingBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private WaitUntil _waitUntil;
    private Coroutine _coroutine;

    protected ParticleSystem ParticleSystem => _particleSystem;

    public void Play()
    {
        ValidateInit(nameof(Play));

        _particleSystem.Play();
        StartCoroutine();
    }

    protected abstract void OnCompleted();

    protected override void OnInitialize() =>
        _waitUntil = new(() => _particleSystem.isStopped);

    private void StartCoroutine()
    {
        StopCoroutine();
        _coroutine = StartCoroutine(WaitForCompletion());
    }

    private void StopCoroutine()
    {
        if (_coroutine == null)
            return;

        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private IEnumerator WaitForCompletion()
    {
        yield return _waitUntil;

        OnCompleted();
    }
}