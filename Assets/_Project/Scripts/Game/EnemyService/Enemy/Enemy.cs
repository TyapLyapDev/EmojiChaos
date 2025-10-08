using System;
using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IPoolable<Enemy>
{
    [SerializeField] private EnemyVisual _visual;
    [SerializeField] private Animator _animator;
    
    private SplineMover _mover;
    private int _id;
    //private EnemyAnimator _customAnimator;

    public event Action<Enemy> Deactivated;

    public int Id => _id;

    public void Initialize(SplineContainer splineContainer)
    {
        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer), "ÎÏÀ×ÊÈ, ÊÎÑßÊ!");

        _visual.Initialize();
        _mover = new(splineContainer, transform);
        //_customAnimator = new(_animator);
    }

    public void SetPositionOffset(float offset)
    {
        if (_mover == null)
            throw new ArgumentException($"{nameof(_mover)} íå áûë èíèöèàëèçèðîâàí");

        _mover.SetSideOffset(offset);
    }

    public void Deactivate()
    {
        _mover.Reset();
        gameObject.SetActive(false);
        Deactivated?.Invoke(this);

    }

    public void Move(float speed, float deltaTime)
    {
        if (_mover != null)
            _mover.Move(speed, deltaTime);
    }

    public void SetColor(Color color) =>
        _visual.SetColor(color);

    public void SetId(int id) =>
        _id = id;
}