using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Game/Level", order = 1)]
public class LevelConfigurations : ScriptableObject
{
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private List<Crowd> _crowds;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isRandomSequence;

    public List<Enemy> EnemyPrefabs => new(_enemyPrefabs);

    public float Speed => _speed;

    private void OnValidate()
    {
        if (GetUniqueCrowdIds().Count > _enemyPrefabs.Count)
            Debug.LogError($"Количество {nameof(_enemyPrefabs)} должно быть не меньше количества уникальных (по Id) {nameof(_crowds)}");
    }

    public List<int> GetUniqueCrowdIds() =>
        _crowds.Select(c => c.Id).Distinct().ToList();

    public List<Crowd> GetCrowdSequence()
    {
        List<Crowd> crowds = _crowds.Where(c => c != null).ToList();

        if(_isRandomSequence)
            Utils.Shuffle(crowds);

        return crowds;
    }
}