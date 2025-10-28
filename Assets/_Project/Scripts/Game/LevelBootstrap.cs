using System;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private Level[] _levels;

    private Level _level;

    private void Awake()
    {
        if (_levels == null || _levels.Length == 0)
            throw new NullReferenceException($"{nameof(_levels)} не назначены в инспекторе!");

        int levelIndex = 0;

        if (levelIndex < 0 || levelIndex >= _levels.Length)
            throw new InvalidOperationException($"Неверный {nameof(levelIndex)}: {levelIndex}");

        _level = Instantiate(_levels[levelIndex]);
        _level.Initialize();
    }

    private void Start() =>
        _level.StartRunning();
}