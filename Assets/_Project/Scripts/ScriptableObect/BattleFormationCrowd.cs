using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Battle formation crowd")]
public class BattleFormationCrowd : ScriptableObject
{
    [SerializeField] private SpawnOrderStrategyType _spawnOrder;

    [SerializeField, Min(0.01f)] private float _delayLine;
    [SerializeField, Min(0)] private float _delayRow;
    [SerializeField, Min(0.01f)] private float _stepOffset;
    [SerializeField, Min(1)] private int _countLines;

    public SpawnOrderStrategyType SpawnOrder => _spawnOrder;

    public float DelayLine => _delayLine;

    public float DelayRow => _delayRow;

    public float StepOffset => _stepOffset;

    public int CountLines => _countLines;
}