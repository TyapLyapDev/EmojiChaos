using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class Level : MonoBehaviour
{
    [SerializeField] private SplineContainer _enemySplineContainer;
    [SerializeField] private SplineContainer _roadSplineContainer;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private List<Color> _colors;
    [SerializeField] private List<Crowd> _crowds;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isRandomSequence;

    private List<AttackSlot> _slots;
    private List<Car> _cars;
    private TypeColorRandomizer _colorRandomizer;
    private EnemyService _enemyService;
    private AttackSystem _attackSystem;
    private CarService _carService;

    private void OnDestroy()
    {
        _enemyService.EnemySpawned -= OnEnemySpawned;

        _enemyService?.Dispose();
        _carService?.Dispose();
        _attackSystem?.Dispose();
    }

    public void Initialize()
    {
        if (_roadSplineContainer.TryGetComponent(out SplineMeshTools.Core.SplineMesh splineMesh))
            splineMesh.GenerateMeshAlongSpline();

        _slots = GetComponentsInChildren<AttackSlot>(true).ToList();
        _cars = GetComponentsInChildren<Car>(true).ToList();

        List<Crowd> crowds = new(_crowds);

        if(_isRandomSequence) 
            Utils.Shuffle(crowds);

        _colorRandomizer = new(new(_colors), GetIds());
        _enemyService = new(this, _enemyPrefab, new(crowds), _enemySplineContainer, _colorRandomizer, _speed);
        _attackSystem = new(new(_slots), _bulletPrefab);
        _carService = new(new(_cars), _colorRandomizer, _attackSystem, _roadSplineContainer);

        _enemyService.EnemySpawned += OnEnemySpawned;
    }

    public void StartRunning() =>
        _enemyService.StartRunning();

    private List<int> GetIds()
    {
        List<int> crowdIds = _crowds.Select(c => c.Id).ToList();
        List<int> carIds = _cars.Select(c => c.Id).ToList();

        return crowdIds.Concat(carIds).ToList();
    }

    private void OnEnemySpawned(Enemy enemy) =>
        _attackSystem.RegisterEnemy(enemy);
}