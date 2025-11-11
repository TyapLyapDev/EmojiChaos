using System;
using UnityEngine;

[Serializable]
public class Crowd
{
    [SerializeField] private SpawnOrderStrategyType _spawnOrder;

    [SerializeField, Min(0.0001f)] private float _delayLine;
    [SerializeField, Min(0)] private float _delayRow;
    [SerializeField, Min(0.0001f)] private float _stepOffset;

    [SerializeField] private int _id;
    [SerializeField, Min(1)] private int _countLines;
    [SerializeField, Min(1)] private int _quantity;

    public SpawnOrderStrategyType SpawnOrder => _spawnOrder;

    public float DelayLine => _delayLine;

    public float DelayRow => _delayRow;

    public float StepOffset => _stepOffset;

    public int Id => _id;

    public int CountLines => _countLines;

    public int Quantity => _quantity;
}