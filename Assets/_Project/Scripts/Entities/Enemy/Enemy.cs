using System;
using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IPoolable<Enemy>
{
    [SerializeField] private EnemyVisual _visual;
    [SerializeField] private Transform _bulletTarget;

    private ParticleShower _particle;
    private EnemyMover _mover;
    private int _id;
    private bool _isInitialized;
    private Color _color;

    public event Action<Enemy> Deactivated;

    public int Type => _id;

    public float SplineDistance => _mover?.CurrentDistance ?? 0f;

    public bool IsActive => gameObject.activeInHierarchy && _isInitialized;

    public Transform BulletTarget => _bulletTarget;

    public Color Color => _color;

    public void Initialize(SplineContainer splineContainer, ParticleShower particleShower)
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

        if (_visual == null)
            throw new NullReferenceException(nameof(_visual));

        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer));

        _particle = particleShower ?? throw new ArgumentNullException(nameof(particleShower));

        _visual.Initialize();
        _mover = new(splineContainer, transform);
        _isInitialized = true;
    }

    public void Activate(int id, float sideOffset, Color color)
    {
        ValidateInitialization(nameof(Activate));

        _id = id;
        _mover.SetSideOffset(sideOffset);
        _visual.SetColor(color);
        _color = color;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        ValidateInitialization(nameof(Deactivate));

        _mover.Reset();
        gameObject.SetActive(false);
        Deactivated?.Invoke(this);
    }

    public void Kill()
    {
        ValidateInitialization(nameof(Kill));

        if (IsActive == false)
            throw new InvalidOperationException($"Объект неактивен");

        _particle.ShowBlood(_bulletTarget.position, _bulletTarget.rotation, _color);
        Deactivate();
    }

    public void Move(float deltaSpeed)
    {
        ValidateInitialization(nameof(Move));

        _mover?.Move(deltaSpeed);
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван до инициализации!");
    }
}