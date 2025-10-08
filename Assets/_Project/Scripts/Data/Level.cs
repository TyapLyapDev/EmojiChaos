using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class Level : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private List<Color> _colors;
    [SerializeField] private List<Crowd> _crowds;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isRandomSequence;

    private List<SlotAttack> _slots;
    private List<Car> _cars;
    private TypeColorRandomizer _colorRandomizer;
    private EnemyService _enemyService;
    private AttackSystem _attackSystem;
    private CarService _carService;

    private void OnDestroy()
    {
        _carService.CarClicked -= OnCarClicked;
        _enemyService.EnemySpawned -= OnEnemySpawned;

        _enemyService?.Dispose();
        _carService?.Dispose();
    }

    public void Initialize()
    {
        _slots = GetComponentsInChildren<SlotAttack>(true).ToList();
        _cars = GetComponentsInChildren<Car>(true).ToList();

        _colorRandomizer = new(new(_colors), GetIds());
        _enemyService = new(this, _enemyPrefab, new(_crowds), _splineContainer, _colorRandomizer, _speed);
        _attackSystem = new(new(_slots), _bulletPrefab);
        _carService = new(new(_cars), _colorRandomizer);

        _carService.CarClicked += OnCarClicked;
        _enemyService.EnemySpawned += OnEnemySpawned;
    }

    public void Run()
    {
        _enemyService.Run();
    }

    private List<int> GetIds()
    {
        List<int> crowdIds = _crowds.Select(c => c.Id).ToList();
        List<int> carIds = _cars.Select(c => c.Id).ToList();

        return crowdIds.Concat(carIds).ToList();
    }

    private void OnCarClicked(Car car)
    {
        if (_attackSystem.TryApply(car.Id, car.BulletCount, car.Color))
            Destroy(car.gameObject);
    }

    private void OnEnemySpawned(Enemy enemy) =>
        _attackSystem.AddEnemy(enemy);
}