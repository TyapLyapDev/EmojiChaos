using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Car : MonoBehaviour, IObstacle
{
    [SerializeField] private CarVisual _visual;
    [SerializeField] private BoxCollider _selfCollider;
    [SerializeField] private int _id;
    [SerializeField] private int _bulletCount;

    private MapSplineNodes _mapSplineNodes;
    private AttackSlot _attackSlot;
    private ICarMovementStrategy _mover;
    private Color _color;
    private bool _isInitialized;

    public int Id => _id;

    public int BulletCount => _bulletCount;

    public bool CanMovement => _mover != null;

    public void Initialize(MapSplineNodes mapSplineNodes)
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

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

        CarForwardMover mover = new(transform, _selfCollider, direction);
        _mover = mover;
        mover.ObstacleCollision += OnObstacleCollision;
        mover.CarRoadDetected += OnCarRoadDetected;

        return true;
    }

    public void Move(float deltaDistance) =>
        _mover?.Move(deltaDistance);

    private void OnObstacleCollision(CarForwardMover carForwardMover)
    {
        _attackSlot.ResetReservation();
        _attackSlot = null;

        carForwardMover.ObstacleCollision -= OnObstacleCollision;
        carForwardMover.CarRoadDetected -= OnCarRoadDetected;

        CarRollBackMover newMover = new(transform, _selfCollider, carForwardMover.Direction);
        _mover = newMover;
        newMover.Stopped += OnRollBackStopped;
    }

    private void OnCarRoadDetected(CarForwardMover carForwardMover, CarRoad road)
    {
        carForwardMover.ObstacleCollision -= OnObstacleCollision;
        carForwardMover.CarRoadDetected -= OnCarRoadDetected;

        CarSplineMover carSplineMover = new(transform, _mapSplineNodes, _attackSlot.transform);
        _mover = carSplineMover;

        carSplineMover.DestinationReached += OnDestinationReached;
        Debug.Log($"Car {Id} reached road {road.name}");
    }

    private void OnRollBackStopped(CarRollBackMover mover)
    {
        mover.Stopped -= OnRollBackStopped;
        _mover = null;
    }

    private void OnDestinationReached(CarSplineMover mover)
    {
        mover.DestinationReached -= OnDestinationReached;
        
        if(_attackSlot.TryActivateGun(_id, _bulletCount, _color) == false)
            throw new Exception($"Не удалось активировать пушку для машины {_id}");

        _mover = null;
        Destroy(gameObject);
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван перед инициализацией. Сначала вызовите {nameof(Initialize)}");
    }
}