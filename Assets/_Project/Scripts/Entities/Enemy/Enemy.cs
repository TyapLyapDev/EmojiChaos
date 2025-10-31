using System;
using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IPoolable<Enemy>
{
    [SerializeField] private EnemyVisual _visual;    
    
    private EnemyMover _mover;
    private int _id = -1;
    private bool _isInitialized;

    public event Action<Enemy> Deactivated;

    public int Type => _id;

    public float SplineDistance => _mover?.CurrentDistance ?? 0f;

    public bool IsActive => gameObject.activeInHierarchy && _isInitialized;

    public void Initialize(SplineContainer splineContainer)
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer));

        if (_visual == null)
            throw new NullReferenceException(nameof(_visual));

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
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        ValidateInitialization(nameof(Deactivate));

        _mover.Reset();
        gameObject.SetActive(false);
        Deactivated?.Invoke(this);
    }

    public void Move(float deltaSpeed)
    {
        ValidateInitialization(nameof(Move));

        _mover?.Move(deltaSpeed);
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван перед инициализацией. Сначала вызовите{nameof(Initialize)}");
    }
}