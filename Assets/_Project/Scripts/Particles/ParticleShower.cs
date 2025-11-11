using System;
using UnityEngine;

public class ParticleShower : IDisposable
{
    private Pool<SmokeParticle> _smokeParticlePool;
    private Pool<BloodParticle> _bloodParticlePool;

    public ParticleShower(Pool<SmokeParticle> smokeParticlePool, Pool<BloodParticle> bloodParticlePool)
    {
        _smokeParticlePool = smokeParticlePool ?? throw new ArgumentNullException(nameof(smokeParticlePool));
        _bloodParticlePool = bloodParticlePool ?? throw new ArgumentNullException(nameof(bloodParticlePool));

        _smokeParticlePool.Created += OnOneShotParticleCreated;
        _bloodParticlePool.Created += OnOneShotParticleCreated;
    }

    public void Dispose()
    {
        if (_smokeParticlePool != null)
            _smokeParticlePool.Created -= OnOneShotParticleCreated;

        if (_bloodParticlePool != null)
            _bloodParticlePool.Created -= OnOneShotParticleCreated;
    }

    public void ShowSmoke(Vector3 position, Quaternion direction)
    {
        if (_smokeParticlePool.TryGive(out SmokeParticle particle))
        {
            particle.SetPositionAndRotation(position, direction);
            particle.SetActive(true);
            particle.Play();
        }
    }

    public void ShowBlood(Vector3 position, Quaternion direction, Color color)
    {
        if (_bloodParticlePool.TryGive(out BloodParticle particle))
        {
            particle.SetPositionAndRotation(position, direction);
            particle.SetColor(color);
            particle.SetActive(true);
            particle.Play();
        }
    }

    private void OnOneShotParticleCreated(OneShotParticle particle)
    {
        if(particle == null)
            throw new ArgumentNullException(nameof(particle));

        particle.Initialize();
    }
}