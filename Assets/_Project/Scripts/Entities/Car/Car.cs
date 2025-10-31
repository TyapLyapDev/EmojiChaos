using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Car : MonoBehaviour, IObstacle, ISwipeable
{
    [SerializeField] private CarVisual _visual;
    [SerializeField] private BoxCollider _selfCollider;
    [SerializeField] private int _id;
    [SerializeField] private int _bulletCount;

    private MapSplineNodes _mapSplineNodes;
    private AttackSlot _attackSlot;
    private IMovementStrategy _mover;
    private Color _color;
    private bool _isInitialized;

    public int Id => _id;

    public int BulletCount => _bulletCount;

    public bool CanMovement => _mover != null;

    public Transform Transform => transform;

    public void Initialize(MapSplineNodes mapSplineNodes)
    {
        if (_isInitialized)
            throw new InvalidOperationException("������� ��������� �������������");

        if (_visual == null)
            throw new NullReferenceException(nameof(_visual));

        _mapSplineNodes = mapSplineNodes ?? throw new ArgumentNullException(nameof(mapSplineNodes));
        _visual.Initialize();
        _isInitialized = true;
    }

    public void SetColor(Color color)
    {
        ValidateInitialization(nameof(SetColor));
        _visual.SetColor(color);
        _color = color;
    }

    public bool TryReservationSlot(AttackSlot attackSlot, int direction)
    {
        if (_mover != null)
            return false;

        _attackSlot = attackSlot;
        _attackSlot.SetReservation();

        CarForwardMoverStrategy mover = new(transform, _selfCollider, direction);
        _mover = mover;
        mover.ObstacleCollision += OnObstacleCollision;
        mover.RoadCarDetected += OnRoadCarDetected;

        return true;
    }

    public void Move(float deltaDistance) =>
        _mover?.Move(deltaDistance);

    private void OnObstacleCollision(CarForwardMoverStrategy carForwardMover)
    {
        _attackSlot.ResetReservation();
        _attackSlot = null;

        carForwardMover.ObstacleCollision -= OnObstacleCollision;
        carForwardMover.RoadCarDetected -= OnRoadCarDetected;

        CarRollBackMoverStrategy newMover = new(transform, _selfCollider, carForwardMover.Direction);
        _mover = newMover;
        newMover.Stopped += OnRollBackStopped;
    }

    private void OnRoadCarDetected(CarForwardMoverStrategy carForwardMover, RoadCar road, Vector3 hitPoint)
    {
        carForwardMover.ObstacleCollision -= OnObstacleCollision;
        carForwardMover.RoadCarDetected -= OnRoadCarDetected;

        List<SplineSegment> path = _mapSplineNodes.GetPathSegments(hitPoint, _attackSlot.GetPosition());
        CarSplineMoverStrategy carSplineMover = new(transform, path);
        _mover = carSplineMover;

        carSplineMover.DestinationReached += OnDestinationReached;
    }

    private void OnRollBackStopped(CarRollBackMoverStrategy mover)
    {
        mover.Stopped -= OnRollBackStopped;
        _mover = null;
    }

    private void OnDestinationReached(CarSplineMoverStrategy mover)
    {
        mover.DestinationReached -= OnDestinationReached;
        
        if(_attackSlot.TryActivateGun(_id, _bulletCount, _color) == false)
            throw new Exception($"�� ������� ������������ ����� ��� ������ {_id}");

        _mover = null;
        Destroy(gameObject);
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"����� {methodName} ��� ������� ����� ��������������. ������� �������� {nameof(Initialize)}");
    }
}