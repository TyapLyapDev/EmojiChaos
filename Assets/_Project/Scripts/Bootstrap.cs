using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LevelConfigurations _levelConfigurations;
    [SerializeField] private EnemySpawner _spawner;
    
    private EnemySpeedDirector _enemySpeedDirector;

    private void Awake()
    {
        _enemySpeedDirector = new(this, _spawner, _levelConfigurations.Speed);
        _enemySpeedDirector.Run();
    }
}