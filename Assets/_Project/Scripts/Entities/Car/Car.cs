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
    private Rack _targetRack;
    private IMovementStrategy _mover;
    private List<SplineSegment> _currentPath;
    private AudioSource _vrum;

    public int Id => _id;

    public int BulletCount => _bulletCount;

    public Transform Transform => transform;

    public void Move(float deltaDistance)
    {
        ValidateInit(nameof(Move));
        _mover?.Move(deltaDistance);
    }

    public bool TryReservationSlot(Rack targetRack, int direction)
    {
        ValidateInit(nameof(TryReservationSlot));

        if (_mover != null)
            return false;

        _targetRack = targetRack;
        _targetRack.SetReservation();

        if (direction < 0)
            _mover = new CarStraightBackwardMoverStrategy(
                transform,
                _selfCollider,
                OnBackwardObstacleCollision,
                OnRoadBackwardDetected);
        else
            _mover = new CarStraightForwardMoverStrategy(
                transform,
                _selfCollider,
                OnForwardObstacleCollision,
                OnRoadForwardDetected);

        _vrum = Audio.Sfx.PlayCarRoar();

        return true;
    }

    public void HanleUnavailableStatus() =>
        _visual.ShowUnavailable();

    protected override void OnInitialize(CarConfig config)
    {
        _config = config;

        _visual.Initialize();
        _visual.SetColor(_config.Color);
    }

    private void FindPath(Vector3 hitPoint) =>
        _currentPath = _config.MapSplineNodes.GetPath(hitPoint, _targetRack.GetPosition());

    private void HandleObstacleCollision(Vector3 hitPoint)
    {
        _targetRack.ResetReservation();
        _targetRack = null;
        _config.ParticleShower.ShowHit(hitPoint);
        _mover = null;

        _vrum.Stop();
        Audio.Sfx.PlayCarAccident();
    }

    private void OnForwardObstacleCollision(Vector3 hitPoint)
    {
        HandleObstacleCollision(hitPoint);
        _visual.ShowForwardAccident(OnForwardAccidentCompleted);
    }

    private void OnBackwardObstacleCollision(Vector3 hitPoint)
    {
        HandleObstacleCollision(hitPoint);
        _visual.ShowBackwardAccident(OnBackwardAccidentCompleted);
    }

    private void OnRoadForwardDetected(CarSplineContainer _, Vector3 hitPoint)
    {
        FindPath(hitPoint);
        _mover = new CarSplineMoverStrategy(transform, _currentPath, OnDestinationReached);
    }

    private void OnRoadBackwardDetected(CarSplineContainer _, Vector3 hitPoint)
    {
        FindPath(hitPoint);
        _mover = new CarSplineRollBackMoverStrategy(transform, _currentPath[0], OnSplineRollBackReached);
    }

    private void OnForwardAccidentCompleted()
    {
        _mover = new CarStraightRollForwardMoverStrategy(transform, _selfCollider, OnRollBackCompleted);
        _config.SpeedDirector.Register(this);
    }

    private void OnBackwardAccidentCompleted()
    {
        _mover = new CarStraightRollBackwardMoverStrategy(transform, _selfCollider, OnRollBackCompleted);
        _config.SpeedDirector.Register(this);
    }

    private void OnRollBackCompleted() =>
        _mover = null;

    private void OnSplineRollBackReached() =>
        _mover = new CarForwardToSplineMoverStrategy(transform, _currentPath[0], OnForwardToSplineMovementCompleted);

    private void OnForwardToSplineMovementCompleted() =>
        _mover = new CarSplineMoverStrategy(transform, _currentPath, OnDestinationReached);

    private void OnDestinationReached()
    {
        if (_targetRack.TryActivateGun(_id, _bulletCount, _config.Color) == false)
            throw new Exception($"Ќе удалось активировать пушку дл€ машины {_id}");

        if (_vrum != null)
            _vrum.Stop();

        Destroy(gameObject);
    }
}