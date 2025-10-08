using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private Level[] _levels;

    private Level _level;

    private void Awake()
    {
        int levelIndex = 0;

        _level = Instantiate(_levels[levelIndex]);
        _level.Initialize();        
    }

    private void Start()
    {
        _level.Run();
    }
}