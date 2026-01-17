using System;
using UnityEngine;

[Serializable]
public class Crowd
{
    [SerializeField] private BattleFormationCrowd _battleFormationCrowd;
    [SerializeField] private int _id;
    [SerializeField, Min(1)] private int _quantity;

    public SpawnOrderStrategyType SpawnOrder => _battleFormationCrowd.SpawnOrder;

    public float DelayLine => _battleFormationCrowd.DelayLine;

    public float DelayRow => _battleFormationCrowd.DelayRow;

    public float StepOffset => _battleFormationCrowd.StepOffset;

    public int CountLines => _battleFormationCrowd.CountLines;

    public int Id => _id;

    public int Quantity => _quantity;
}
