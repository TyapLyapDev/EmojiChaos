using System;
using UnityEngine;

public class Star : InitializingWithConfigBehaviour<StarParam>
{
    private const float FearProgressDistance = 0.1f;
    private const float RelaxProgressDistance = 0.22f;

    [SerializeField] private StarVisual _visual;
    [SerializeField] private float _progressOnSpline = 1f;
    [SerializeField] private Transform _centerTarget;

    private bool _isFear = false;

    private StarParam _config;

    public event Action<Star> Destroyed;

    public Transform Center => _centerTarget;

    private void OnDestroy()
    {
        if (_config.EnemySpeedDirector != null)
            _config.EnemySpeedDirector.FirstEnemyProgressChanged -= OnFirstEnemyProgressChanged;
    }

    public void SetProgress(float progress) =>
        _progressOnSpline = progress;

    public void SetEnjoy()
    {
        if (_isFear == false)
            return;

        _isFear = false;
        _visual.SetEnjoy();

        if (Audio.Sfx != null)
            Audio.Sfx.PlayStarRelax();
    }

    protected override void OnInitialize(StarParam config)
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
            Audio.Sfx.PlayStarByeBye();

            return;
        }

        if (progress >= _progressOnSpline - FearProgressDistance)
        {
            if (_isFear)
                return;

            _isFear = true;
            _visual.SetFear();
            Audio.Sfx.PlayStarFear();

            return;
        }

        if (progress <= _progressOnSpline - RelaxProgressDistance)
            SetEnjoy();
    }

    private void OnDiedCompleted()
    {
        _visual.DiedCompleted -= OnDiedCompleted;
        _config.ParticleShower.ShowStarBang(_centerTarget.position, _centerTarget.rotation);
        _config.CameraShaker.Shake();
        Destroyed?.Invoke(this);
        Audio.Sfx.PlayStarPoof();
        Destroy(gameObject);
    }
}