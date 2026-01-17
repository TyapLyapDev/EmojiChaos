using UnityEngine;

public class GunExplanation : TutorialItem
{
    [SerializeField] private TutorialCircle _circle;
    [SerializeField] private float _circleSize = 1;

    private Gun _gun;

    protected override void OnActivated() =>
        SubscribeRacks();

    protected override void OnDeactivated()
    {
        UnsubscribeRacks();

        _circle.AnyClicked -= Deactivate;
        Config.SwipeStrategy.Resume();
        Config.EnemySpawner.Resume();
        Config.EnemiesSpeedDirector.Resume();
        _circle.Hide();
        Hide();
    }

    private void SubscribeRacks()
    {
        foreach (Rack rack in Config.Racks)
            rack.GunInstalled += OnGunInstalled;
    }

    private void UnsubscribeRacks()
    {
        foreach (Rack rack in Config.Racks)
            rack.GunInstalled -= OnGunInstalled;
    }

    private void OnGunInstalled(Gun gun)
    {
        if (IsActivated == false)
            return;

        _gun = gun;

        UnsubscribeRacks();

        Config.SwipeStrategy.Pause();
        Config.EnemySpawner.Pause();
        Config.EnemiesSpeedDirector.Pause();
        _circle.Show(_circleSize, _gun.Center.position);
        _circle.AnyClicked += Deactivate;
        Show();
    }
}