using UnityEngine;

public class CarExplanation : TutorialItem
{
    [SerializeField] private TutorialCircle _circle;
    [SerializeField] private float _circleSize = 1;

    protected override void OnActivated()
    {
        Config.SwipeStrategy.Pause();
        Config.EnemySpawner.Pause();
        Config.EnemiesSpeedDirector.Pause();
        _circle.Show(_circleSize, Config.Car.GetPosition());
        _circle.AnyClicked += Deactivate;
        Show();
    }

    protected override void OnDeactivated()
    {
        _circle.AnyClicked -= Deactivate;
        Config.SwipeStrategy.Resume();
        Config.EnemySpawner.Resume();
        Config.EnemiesSpeedDirector.Resume();
        _circle.Hide();
        Hide();
    }
}
