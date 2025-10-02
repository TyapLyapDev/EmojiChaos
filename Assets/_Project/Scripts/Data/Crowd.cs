using System;
using UnityEngine;

[Serializable]
public class Crowd
{
    [SerializeField] private SpawnOrderType _spawnOrder;

    [SerializeField, Min(0.0001f)] private float _delayLine;
    [SerializeField] private float _delayRow;
    [SerializeField, Min(0.0001f)] private float _stepOffset;

    [SerializeField] private int _id;
    [SerializeField] private int _countLines;
    [SerializeField] private int _quantity;

    public SpawnOrderType SpawnOrder => _spawnOrder;

    public float DelayLine => _delayLine;

    public float DelayRow => _delayRow;

    public float StepOffset => _stepOffset;

    public int Id => _id;

    public int CountLines => _countLines;

    public int Quantity => _quantity;
}