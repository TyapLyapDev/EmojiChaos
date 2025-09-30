using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SplineContainer _spline;
    [SerializeField] private Enemy _prefab;
    [SerializeField, Min(1)] private int _maximumPoolSize;
    [SerializeField, Min(0.01f)] private float _delayInSeconds;
    [SerializeField, Min(0.01f)] private float _stepOffset;
    [SerializeField] private float[] _offsets;
    [SerializeField] private int _countLines;

    private Pool<Enemy> _pool;
    private Transform _parent;
    private Coroutine _coroutine;
    private WaitForSeconds _wait;
    private int _remainingQuantity = 30;
    private int[] _spawnOrder;
    private int _currentIndex;

    public event Action Spawned;

    private void OnValidate() =>
        CalculateOffsets();

    private void Awake()
    {
        _parent = _spline.transform;
        _pool = new(_prefab, _parent, _maximumPoolSize);
        _wait = new(_delayInSeconds);

        CalculateOffsets();
        _spawnOrder = CalculateSpawnOrder(_countLines);
    }

    private void Start() =>
        StartSpawn();

    public void StartSpawn()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        while(_remainingQuantity > 0)
        {
            if (_pool.TryGet(out Enemy enemy))
            {
                enemy.Initialize(_spline, _offsets[_spawnOrder[_currentIndex]]);
                _currentIndex = (_currentIndex + 1) % _offsets.Length;
                _remainingQuantity--;
            }

            yield return _wait;
        }        
    }

    private void CalculateOffsets()
    {
        _offsets = new float[_countLines];

        if (_countLines == 1)
        {
            _offsets[0] = 0f;
        }
        else
        {
            float startOffset = -(_countLines - 1) * _stepOffset / 2f;

            for (int i = 0; i < _countLines; i++)
                _offsets[i] = startOffset + i * _stepOffset;
        }
    }

    private int[] CalculateSpawnOrder(int countLines)
    {
        int[] spawnOrder = new int[countLines];

        if (countLines == 1)
        {
            spawnOrder[0] = 0;
            return spawnOrder;
        }

        int centerIndex = countLines / 2;
        if (countLines % 2 == 0)
        {
            centerIndex = countLines / 2 - 1;
        }

        spawnOrder[0] = centerIndex;

        int leftDistance = 1;
        int rightDistance = 1;
        int orderIndex = 1;

        while (orderIndex < countLines)
        {
            int leftIndex = centerIndex - leftDistance;
            if (leftIndex >= 0 && orderIndex < countLines)
            {
                spawnOrder[orderIndex] = leftIndex;
                orderIndex++;
                leftDistance++;
            }

            int rightIndex = centerIndex + rightDistance;
            if (rightIndex < countLines && orderIndex < countLines)
            {
                spawnOrder[orderIndex] = rightIndex;
                orderIndex++;
                rightDistance++;
            }
        }

        return spawnOrder;
    }
}