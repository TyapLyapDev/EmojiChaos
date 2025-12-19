using UnityEngine;

public class Tutorial : InitializingWithConfigBehaviour<TutorialConfig>
{
    [SerializeField] TutorialCircle _circle;
    [SerializeField] private GameObject _portalExplanation;
    [SerializeField] private GameObject _enemyExplanation;
    [SerializeField] private GameObject _carExplanation;
    [SerializeField] private GameObject _gunExplanation;
    [SerializeField] private GameObject _starExplanation;

    private TutorialConfig _config;
    private Enemy _firstEnemy;
    private Gun _gun;

    protected override void OnInitialize(TutorialConfig config)
    {
        _config = config;

        _portalExplanation.SetActive(false);
        _enemyExplanation.SetActive(false);
        _carExplanation.SetActive(false);
        _gunExplanation.SetActive(false);
        _starExplanation.SetActive(false);
        _circle.Initialize();
        _circle.Hide();

        PauseEnemy();
        _config.SwipeStrategy.Pause();

        Invoke(nameof(ShowPortalExplanation), 1);
    }

    private void PauseEnemy()
    {
        _config.EnemySpawner.Pause();
        _config.EnemiesSpeedDirector.Pause();
    }

    private void ResumeEnemy()
    {
        _config.EnemySpawner.Resume();
        _config.EnemiesSpeedDirector.Resume();
    }

    private void ShowPortalExplanation()
    {
        _portalExplanation.SetActive(true);
        _circle.Show(1, _config.PortalTarget.position);
        _circle.AnyClicked += HidePanelWithPortalExplanation;
    }

    private void HidePanelWithPortalExplanation()
    {
        _portalExplanation.SetActive(false);
        _circle.AnyClicked -= HidePanelWithPortalExplanation;
        _circle.Hide();
        ResumeEnemy();
        _config.EnemySpawner.Spawned += OnEnemySpawned;
    }

    private void ShowEnemyExplanation()
    {
        _enemyExplanation.SetActive(true);
        _circle.Show(1.7f, _firstEnemy.CenterBody.position);
        PauseEnemy();
        _circle.AnyClicked += HideEnemyExplanation;
    }

    private void HideEnemyExplanation()
    {
        _circle.AnyClicked -= HideEnemyExplanation;
        _enemyExplanation.SetActive(false);
        ShowCarExplanation();
    }

    private void ShowCarExplanation()
    {
        _circle.Show(1.2f, _config.Car.GetPosition());
        _carExplanation.SetActive(true);
        _config.SwipeStrategy.Resume();
        _circle.AnyClicked += HideCarExplanation;
    }

    private void HideCarExplanation()
    {
        _circle.AnyClicked -= HideCarExplanation;
        _carExplanation.SetActive(false);
        _circle.Hide();
        _config.SwipeStrategy.HasSwipe += OnSwipe;
    }

    private void ShowGunExplanation()
    {
        PauseEnemy();
        _gunExplanation.SetActive(true);
        _circle.Show(1.8f, _gun.Center.position);
        _circle.AnyClicked += HideGunExplanation;
    }

    private void HideGunExplanation()
    {
        _circle.AnyClicked -= HideGunExplanation;
        _gunExplanation.SetActive(false);
        ShowStarExplanation();
    }

    private void ShowStarExplanation()
    {
        _circle.AnyClicked += HideStarExplanation;
        _starExplanation.SetActive(true);
        _circle.Show(1f, _config.Star.Center.position);
    }

    private void HideStarExplanation()
    {
        _circle.AnyClicked -= HideStarExplanation;
        _starExplanation.SetActive(false);
        _circle.Hide();
        ResumeEnemy();
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        _config.EnemySpawner.Spawned -= OnEnemySpawned;
        _firstEnemy = enemy;
        Invoke(nameof(ShowEnemyExplanation), 3);
    }

    private void OnSwipe(ISwipeable swipeable, int arg2)
    {
        _config.SwipeStrategy.HasSwipe -= OnSwipe;
        ResumeEnemy();

        foreach (Rack rack in _config.Racks)
            rack.GunInstalled += OnGunInstalled;
    }

    private void OnGunInstalled(Gun gun)
    {
        _gun = gun;

        foreach (Rack rack in _config.Racks)
            rack.GunInstalled -= OnGunInstalled;

        ShowGunExplanation();
    }
}