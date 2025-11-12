using System;
using UnityEngine;

public class Enemy : InitializingWithConfigBehaviour<EnemyConfig>, IPoolable<Enemy>, IHittable
{
    [SerializeField] private EnemyVisual _visual;
    [SerializeField] private Transform _center;

    private EnemyConfig _config;
    private EnemyMover _mover;
    private int _id;
    private Color _color;

    public event Action<Enemy> Deactivated;
    public event Action<IHittable> Disappeared;

    public int Type => _id;

    public float SplineDistance => _mover?.CurrentDistance ?? 0f;

    public bool IsActive => GetSafeValue(gameObject.activeInHierarchy);

    public Transform Center => _center;

    public Color Color => _color;

    public void Activate(int id, float sideOffset, Color color)
    {
        ValidateInit(nameof(Activate));

        _id = id;
        _mover.SetSideOffset(sideOffset);
        _visual.SetColor(color);
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

    public void Kill()
    {
        ValidateInit(nameof(Kill));

        if (IsActive == false)
            throw new InvalidOperationException($"Объект неактивен");

        _config.ParticleShower.ShowBlood(_center.position, _center.rotation, _color);
        Deactivate();
    }

    public void Move(float deltaSpeed)
    {
        ValidateInit(nameof(Move));

        _mover?.Move(deltaSpeed);
    }

    protected override void OnInitialize(EnemyConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        _visual.Initialize();
        _mover = new(_config.SplineContainer, transform);
    }
}