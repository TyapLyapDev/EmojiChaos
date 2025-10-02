using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour
{
    private const int MaximumPoolSize = 300;

    [SerializeField] private SplineContainer _spline;
    [SerializeField] private LevelConfigurations _levelConfigurations;

    private readonly LinesOffsetCalculator _offsetsCalculator = new();
    private readonly SpawnStrategyRegistry _spawnStrategy = new();

    private Dictionary<Enemy, Pool<Enemy>> _poolsByPrefab;
    private Dictionary<int, Pool<Enemy>> _poolsByCrowdId;
    private List<Crowd> _crowds;
    private Transform _parent;
    private Coroutine _coroutine;

    public event Action<Enemy> Spawned;

    private void Awake() =>
        Initialize();

    private void Start() =>
        StartSpawn();

    private void Initialize()
    {
        _poolsByPrefab = new Dictionary<Enemy, Pool<Enemy>>();
        _poolsByCrowdId = new Dictionary<int, Pool<Enemy>>();
        _parent = _spline.transform;

        foreach (Enemy prefab in _levelConfigurations.EnemyPrefabs)
            _poolsByPrefab[prefab] = new Pool<Enemy>(prefab, _parent, MaximumPoolSize);

        _crowds = _levelConfigurations.GetCrowdSequence();

        AssignPoolsToCrowdIds();
    }

    public void StartSpawn()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Spawning());
    }

    private void AssignPoolsToCrowdIds()
    {
        List<Pool<Enemy>> availablePools = new(_poolsByPrefab.Values);

        foreach (var crowdId in _levelConfigurations.GetUniqueCrowdIds())
        {
            if (availablePools.Count == 0)
                availablePools = new List<Pool<Enemy>>(_poolsByPrefab.Values);

            int randomIndex = UnityEngine.Random.Range(0, availablePools.Count);
            _poolsByCrowdId[crowdId] = availablePools[randomIndex];
            availablePools.RemoveAt(randomIndex);
        }
    }

    private IEnumerator Spawning()
    {
        float levelSpeed = _levelConfigurations.Speed;
        List<Pool<Enemy>> availablePools = new(_poolsByPrefab.Values);

        while (_crowds.Count > 0)
        {
            Crowd crowd = _crowds.First();

            if (crowd == null)
                throw new ArgumentNullException(nameof(crowd), "ХОБА!");

            if (_poolsByCrowdId.TryGetValue(crowd.Id, out Pool<Enemy> selectedPool) == false)
            {
                Debug.LogError($"Не найден пул для толпы с ID: {crowd.Id}");
                _crowds.RemoveAt(0);

                continue;
            }

            int currentIndex = 0;
            int countLines = crowd.CountLines;
            float stepOffset = crowd.StepOffset;
            float[] offsets = _offsetsCalculator.Calculate(countLines, stepOffset);

            float normalizedLineTime = crowd.DelayLine / levelSpeed;
            WaitForSeconds waitLine = new(normalizedLineTime);

            float normalizedRowTime = crowd.DelayRow / levelSpeed;
            WaitForSeconds waitRow = new(normalizedRowTime);

            int[] spawnOrder = _spawnStrategy.CalculateSpawnOrder(crowd.SpawnOrder, countLines);
            int remainingQuantity = crowd.Quantity;

            while (remainingQuantity > 0)
            {
                if (selectedPool.TryGet(out Enemy enemy))
                {
                    enemy.Initialize(_spline, offsets[spawnOrder[currentIndex]]);
                    currentIndex = (currentIndex + 1) % offsets.Length;
                    remainingQuantity--;
                    Spawned?.Invoke(enemy);
                }

                bool isLastInLine = (currentIndex % countLines == 0) && (remainingQuantity > 0);

                yield return isLastInLine ? waitLine : waitRow;
            }

            yield return waitLine;

            _crowds.RemoveAt(0);
        }
    }
}