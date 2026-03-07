using System;
using UnityEngine;

public class ParticleShower
{
    private readonly Pool<SmokeParticle> _smokeParticlePool;
    private readonly Pool<BloodParticle> _bloodParticlePool;
    private readonly Pool<StarBangParticle> _starBangParticlePool;
    private readonly Pool<HitParticle> _hitParticlePool;

    public ParticleShower(
        Pool<SmokeParticle> smokeParticlePool,
        Pool<BloodParticle> bloodParticlePool,
        Pool<StarBangParticle> starBangParticlePool,
        Pool<HitParticle> hitParticlePool)
    {
        _smokeParticlePool = smokeParticlePool ?? throw new ArgumentNullException(nameof(smokeParticlePool));
        _bloodParticlePool = bloodParticlePool ?? throw new ArgumentNullException(nameof(bloodParticlePool));
        _starBangParticlePool = starBangParticlePool ?? throw new ArgumentNullException(nameof(starBangParticlePool));
        _hitParticlePool = hitParticlePool ?? throw new ArgumentNullException(nameof(hitParticlePool));
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

    public void ShowStarBang(Vector3 position, Quaternion direction)
    {
        if (_starBangParticlePool.TryGive(out StarBangParticle particle))
        {
            particle.SetPositionAndRotation(position, direction);
            particle.SetActive(true);
            particle.Play();
        }
    }

    public void ShowHit(Vector3 position)
    {
        if (_hitParticlePool.TryGive(out HitParticle particle))
        {
            particle.SetPositionAndRotation(position, Quaternion.identity);
            particle.SetActive(true);
            particle.Play();
        }
    }
}