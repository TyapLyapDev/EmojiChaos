using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour, IPoolable<Bullet>
{
    [SerializeField] private BulletVisual _visual;
    [SerializeField] private float _speed;

    private BulletMover _mover;
    private Enemy _target;
    private Transform _targetTransform;

    public event Action<Bullet> Deactivated;

    private void Update() =>
        _mover.Move(_targetTransform, _speed * Time.deltaTime);

    private void OnDestroy()
    {
        _mover.TargetReached -= OnTargetReached;
    }

    public void Initialize()
    {
        _visual.Initialize();
        _mover = new(transform);
        _mover.TargetReached += OnTargetReached;
    }    

    public void Activate(Enemy target, Color color)
    {
        _target = target;
        _targetTransform = _target.transform;
        _visual.SetColor(color);

        target.Deactivated += OnTargetDeactivated;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _target = null;
        _targetTransform = null;
        gameObject.SetActive(false);
        Deactivated?.Invoke(this);
    }

    private void OnTargetReached() =>
        _target.Deactivate();        

    private void OnTargetDeactivated(Enemy enemy)
    {
        enemy.Deactivated -= OnTargetDeactivated;
        Deactivate();
    }
}