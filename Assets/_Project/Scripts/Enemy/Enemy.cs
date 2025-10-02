using System;
using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IPoolable<Enemy>
{
    [SerializeField] private Animator _animator;

    private SplineMover _mover;
    //private EnemyAnimator _customAnimator;

    public event Action<Enemy> Deactivated;

    public void Initialize(SplineContainer splineContainer, float offset)
    {
        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer), "Œœ¿◊ »,  Œ—ﬂ !");

        _mover = new(splineContainer, transform);
        _mover.SetSideOffset(offset);
        _mover.SetSideOffset(offset);
        //_customAnimator = new(_animator);
    }

    public void Move(float speed, float deltaTime)
    {
        if (_mover != null)
            _mover.Move(speed, deltaTime);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Deactivated?.Invoke(this);
    }
}