using System;
using UnityEngine;

public class Star : InitializingWithConfigBehaviour<StarConfig>
{
    private const float FearProgressDistance = 0.2f;

    [SerializeField] private StarVisual _visual;
    [SerializeField] private float _progressOnSpline = 1f;
    [SerializeField] private Transform _centerTarget;

    private bool _isFear = false;

    private StarConfig _config;

    private void OnDestroy()
    {
        if (_config.EnemySpeedDirector != null)
            _config.EnemySpeedDirector.FirstEnemyProgressChanged -= OnFirstEnemyProgressChanged;
    }

    public void SetProgress(float progress) =>
        _progressOnSpline = progress;

    protected override void OnInitialize(StarConfig config)
    {
        _visual.Initialize();
        _config = config;
        _config.EnemySpeedDirector.FirstEnemyProgressChanged += OnFirstEnemyProgressChanged;
    }

    private void OnFirstEnemyProgressChanged(float progress)
    {
        if (progress >= _progressOnSpline)
        {
            _config.EnemySpeedDirector.FirstEnemyProgressChanged -= OnFirstEnemyProgressChanged;
            _visual.DiedCompleted += OnDiedCompleted;

            _visual.SetDied();
            _config.CameraShaker.Shake();

            return;
        }

        if (progress >= _progressOnSpline - FearProgressDistance)
        {
            if (_isFear)
                return;

            _isFear = true;
            _visual.SetFear();

            return;
        }

        if (_isFear == false)
            return;

        _isFear = false;
        _visual.SetEnjoy();
    }

    private void OnDiedCompleted()
    {
        _visual.DiedCompleted -= OnDiedCompleted;
        _config.ParticleShower.ShowStarBang(_centerTarget.position, _centerTarget.rotation);
        Destroy(gameObject);
    }
}