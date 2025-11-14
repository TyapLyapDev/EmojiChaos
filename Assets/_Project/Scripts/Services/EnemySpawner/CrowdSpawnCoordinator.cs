using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawnCoordinator : IDisposable
{
    private readonly EnemySpawner _enemySpawner;
    private readonly CrowdSequencer _crowdSequencer;
    private readonly MonoBehaviour _runner;
    private readonly ParticleSystem _portal;

    private Coroutine _crowdCoroutine;
    private Coroutine _enemySpawningCoroutine;

    public CrowdSpawnCoordinator(MonoBehaviour runner, EnemySpawner enemySpawner, List<Crowd> crowdSequence, ParticleSystem portal)
    {
        _runner = runner != null ? runner : throw new ArgumentNullException(nameof(runner));
        _enemySpawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
        _crowdSequencer = new(crowdSequence);
        _portal = portal != null ? portal : throw new ArgumentNullException(nameof(portal));
    }

    public void Dispose()
    {
        StopRunning();
    }

    public void Run()
    {
        StopRunning();
        _crowdCoroutine = _runner.StartCoroutine(Spawning());
    }

    public void StopRunning()
    {
        if (_crowdCoroutine != null)
            _runner.StopCoroutine(_crowdCoroutine);

        if (_enemySpawningCoroutine != null)
            _runner.StopCoroutine(_enemySpawningCoroutine);
    }

    private IEnumerator Spawning()
    {
        while (_crowdSequencer.TryGiveNextCrowd(out Crowd crowd))
            yield return _enemySpawningCoroutine = _runner.StartCoroutine(_enemySpawner.SpawnCrowd(crowd));

        _portal.Stop();
    }
}