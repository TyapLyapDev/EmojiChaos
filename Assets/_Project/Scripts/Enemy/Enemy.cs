using System;
using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IPoolable<Enemy>
{
    [SerializeField] private Animator _animator;

    private SplineMover _mover;
    //private EnemyAnimator _customAnimator;

    public event Action<Enemy> Deactivated;

    public void Initialize(SplineContainer splineContainer)
    {
        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer), "ќѕј„ »,  ќ—я !");

        _mover = new(splineContainer, transform);
        //_customAnimator = new(_animator);
    }

    public void SetPositionOffset(float offset)
    {
        if (_mover == null)
            throw new ArgumentException($"{nameof(_mover)} не был инициализирован");

        _mover.SetSideOffset(offset);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Deactivated?.Invoke(this);
    }

    public void Move(float speed, float deltaTime)
    {
        if (_mover != null)
            _mover.Move(speed, deltaTime);
    }
}