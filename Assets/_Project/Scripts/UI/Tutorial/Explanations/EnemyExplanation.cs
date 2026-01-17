using UnityEngine;

public class EnemyExplanation : TutorialItem
{
    [SerializeField] private TutorialCircle _circle;
    [SerializeField] private float _circleSize = 1;
    [SerializeField] private float _delay = 1;

    private Enemy _enemy;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Config.EnemySpawner.Spawned += OnEnemySpawned;
    }

    protected override void OnActivated()
    {        
        Config.SwipeStrategy.Pause();
        Invoke(nameof(HandleActivation), _delay);
    }

    protected override void OnDeactivated()
    {
        Config.EnemySpawner.Spawned -= OnEnemySpawned;
        _circle.AnyClicked -= Deactivate;

        Config.EnemySpawner.Resume();
        Config.EnemiesSpeedDirector.Resume();
        Config.SwipeStrategy.Resume();
        _circle.Hide();
        Hide();
    }

    private void HandleActivation()
    {
        if (IsActivated == false)
            return;

        _circle.AnyClicked += Deactivate;
        Config.EnemySpawner.Pause();
        Config.EnemiesSpeedDirector.Pause();
        _circle.Show(_circleSize, _enemy.CenterBody.position);
        Show();
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        if (IsActivated == false)
            return;

        Config.EnemySpawner.Spawned -= OnEnemySpawned;
        _enemy = enemy;
    }
}