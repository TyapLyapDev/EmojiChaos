using UnityEngine;

public class PortalExplanation : TutorialItem
{
    [SerializeField] private TutorialCircle _circle;
    [SerializeField] private float _circleSize = 1;
    [SerializeField] private float _delay = 1;

    protected override void OnActivated()
    {
        Config.EnemySpawner.Pause();
        Config.EnemiesSpeedDirector.Pause();
        Config.SwipeStrategy.Pause();

        Invoke(nameof(HandleActivation), _delay);
    }

    protected override void OnDeactivated()
    {
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

        Show();
        _circle.AnyClicked += Deactivate;
        _circle.Show(_circleSize, Config.PortalTarget.position);
    }
}