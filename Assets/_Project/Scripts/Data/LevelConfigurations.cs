using System;
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
            throw new Exception($"Количество {nameof(_enemyPrefabs)} должно быть не меньше количества уникальных (по Id) {nameof(_crowds)}");
    }

    public List<int> GetUniqueCrowdIds() =>
        _crowds.Select(c => c.Id).Distinct().ToList();

    public List<Crowd> GetCrowdSequence() =>
        _isRandomSequence ? GetShuffledCrowds() : _crowds.Where(c => c != null).ToList();

    private List<Crowd> GetShuffledCrowds()
    {
        if (_crowds == null || _crowds.Count == 0)
            return new List<Crowd>();

        List<Crowd> shuffledList = new(_crowds);

        for (int i = shuffledList.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            Crowd temp = shuffledList[i];
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        return shuffledList;
    }
}