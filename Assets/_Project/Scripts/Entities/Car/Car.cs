using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Car : InitializingWithConfigBehaviour<CarConfig>, IObstacle, ISwipeable
{
    [SerializeField] private CarVisual _visual;
    [SerializeField] private BoxCollider _selfCollider;
    [SerializeField] private int _id;
    [SerializeField] private int _bulletCount;

    private CarConfig _config;
    private Rack _attackSlot;
    private IMovementStrategy _mover;

    public int Id => _id;

    public int BulletCount => _bulletCount;

    public Transform Transform => transform;

    public bool TryReservationSlot(Rack attackSlot, int direction)
    {
        ValidateInit(nameof(TryReservationSlot));

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

    public void Move(float deltaDistance)
    {
        ValidateInit(nameof(TryReservationSlot));

        _mover?.Move(deltaDistance);
    }

    protected override void OnInitialize(CarConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        _visual.Initialize();
        _visual.SetColor(_config.Color);
    }

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

    private void OnRoadCarDetected(CarForwardMoverStrategy carForwardMover, CarSplineContainer road, Vector3 hitPoint)
    {
        carForwardMover.ObstacleCollision -= OnObstacleCollision;
        carForwardMover.RoadCarDetected -= OnRoadCarDetected;

        List<SplineSegment> path = _config.MapSplineNodes.GetPathSegments(hitPoint, _attackSlot.GetPosition());
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
        
        if(_attackSlot.TryActivateGun(_id, _bulletCount, _config.Color) == false)
            throw new Exception($"Ќе удалось активировать пушку дл€ машины {_id}");

        Destroy(gameObject);
    }
}