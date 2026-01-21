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

    private AudioSource _roar;
    private ParticleSystem _smoke;
    private Rack _targetRack;
    private CarConfig _config;
    private IMovementStrategy _mover;
    private List<SplineSegment> _currentPath;

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

        _roar = Audio.Sfx.PlayCarRoar();

        return true;
    }

    public void HanleUnavailableStatus() =>
        _visual.ShowUnavailable();

    public void DisableSmoke()
    {
        if (_smoke != null)
            _smoke.gameObject.SetActive(false);
    }

    public void EnableSmoke()
    {
        if (_smoke != null)
            _smoke.gameObject.SetActive(true);
    }

    protected override void OnInitialize(CarConfig config)
    {
        _config = config;

        _visual.Initialize();
        _visual.SetColor(_config.Color);

        _smoke = GetComponentInChildren<ParticleSystem>();
        DisableSmoke();
    }

    private void FindPath(Vector3 hitPoint) =>
        _currentPath = _config.MapSplineNodes.GetPath(hitPoint, _targetRack.GetPosition());

    private void HandleObstacleCollision(Vector3 hitPoint)
    {
        _targetRack.ResetReservation();
        _targetRack = null;
        _config.ParticleShower.ShowHit(hitPoint);
        _mover = null;
        DisableSmoke();

        if (_roar != null)
        {
            _roar.Stop();
            _roar = null;
        }
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

        if (_roar != null)
            _roar.Stop();

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Vector3 position = transform.position + Vector3.up * 0.3f + Vector3.forward * -0.14f;
        string text = _id.ToString();

        float bgRadius = 0.1f;
        UnityEditor.Handles.color = new Color(0.1f, 0.1f, 0.1f, 0.5f);
        UnityEditor.Handles.DrawSolidDisc(position, Camera.current?.transform.forward ?? Vector3.forward, bgRadius);

        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(position, Camera.current?.transform.forward ?? Vector3.forward, bgRadius);

        GUIStyle style = new()
        {
            fontSize = 12,
            normal = { textColor = Color.yellow },
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        UnityEditor.Handles.Label(position, text, style);
#endif
    }
}