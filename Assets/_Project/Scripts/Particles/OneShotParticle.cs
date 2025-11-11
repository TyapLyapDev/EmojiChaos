using System;
using System.Collections;
using UnityEngine;

public abstract class OneShotParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private WaitUntil _waitUntil;
    private Coroutine _coroutine;
    private bool _isInitialized;

    protected ParticleSystem ParticleSystem => _particleSystem;

    public virtual void Initialize()
    {
        _waitUntil = new WaitUntil(() => _particleSystem.isStopped);
        _isInitialized = true;
    }

    public void Play()
    {
        ValidateInitialization(nameof(Play));

        _particleSystem.Play();
        StartCoroutine();
    }

    protected abstract void OnComleted();

    private void StartCoroutine()
    {
        StopCoroutine();
        _coroutine = StartCoroutine(WaitForCompletion());
    }

    private void StopCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private IEnumerator WaitForCompletion()
    {
        yield return _waitUntil;

        OnComleted();
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван до инициализации. Сначала вызовите {nameof(Initialize)}");
    }
}