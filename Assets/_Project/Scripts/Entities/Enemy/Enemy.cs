using System;
using UnityEngine;

public class Enemy : InitializingWithConfigBehaviour<EnemyParam>, IPoolable<Enemy>, IHittable
{
    [SerializeField] private EnemyVisual _visual;
    [SerializeField] private Transform _centerBody;

    private EnemyParam _config;
    private EnemyMover _mover;
    private int _id;
    private Color _color;

    public event Action<Enemy> Killed;
    public event Action<Enemy> Deactivated;
    public event Action<IHittable> Disappeared;

    public int Type => _id;

    public float SplineDistance => _mover?.CurrentDistance ?? 0f;

    public bool IsActive => GetSafeValue(gameObject.activeInHierarchy);

    public Transform CenterBody => _centerBody;

    public Color Color => _color;

    public float Progress => _mover?.Progress ?? 0f;

    private void OnDestroy()
    {
        _visual.DiedCompleted -= OnDiedCompleted;
        Deactivated?.Invoke(this);
    }

    public void Activate(int id, float sideOffset, Color color)
    {
        ValidateInit(nameof(Activate));

        _id = id;
        _mover.SetSideOffset(sideOffset);
        _visual.SetColor(color);
        _visual.ResetDied();
        _color = color;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        ValidateInit(nameof(Deactivate));

        _mover.Reset();
        gameObject.SetActive(false);
        Disappeared?.Invoke(this);
        Deactivated?.Invoke(this);
    }

    public void Move(float deltaSpeed)
    {
        ValidateInit(nameof(Move));

        _mover?.Move(deltaSpeed);
    }

    public void Scare()
    {
        _visual.SetFear();
    }

    public void Kill()
    {
        ValidateInit(nameof(Kill));

        if (IsActive == false)
            throw new InvalidOperationException($"Объект неактивен");

        _visual.SetDied();
        Killed?.Invoke(this);

        Audio.Sfx.PlayEnemyHit();
    }

    protected override void OnInitialize(EnemyParam config)
    {
        _config = config;

        _visual.Initialize();
        _mover = new(_config.SplineContainer, transform);

        _visual.DiedCompleted += OnDiedCompleted;
    }


    private void OnDiedCompleted()
    {
        _config.ParticleShower.ShowBlood(_centerBody.position, _centerBody.rotation, _color);
        Deactivate();
    }
}