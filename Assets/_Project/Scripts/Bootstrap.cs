using UnityEngine;
using UnityEngine.Splines;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LevelConfigurations _levelConfigurations;
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private EnemySpawner _spawner;
    
    private EnemySpeedDirector _enemySpeedDirector;

    private void Awake()
    {
        _spawner = new(this, _levelConfigurations, _splineContainer);
        _enemySpeedDirector = new(this, _spawner, _levelConfigurations.Speed);

        _enemySpeedDirector.Run();
        _spawner.StartSpawning(_levelConfigurations.Speed);
    }

    private void OnDestroy()
    {
        _spawner?.Dispose();
        _enemySpeedDirector?.Dispose();
    }
}