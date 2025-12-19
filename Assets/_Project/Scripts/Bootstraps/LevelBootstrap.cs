using System;
using System.Linq;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private LevelUiHandler _uiHandler;

    private Level _level;
    private Tutorial _tutorial;
    private readonly ServicesRegistry _services = new();

    private void Start()
    {
        CreateLevel();
        RegisterServices();
        InitializeComponents();
        StartRun();
    }

    private void OnDestroy() =>
        _services?.Dispose();

    private void CreateLevel()
    {
        Saver saver = new(Utils.CalculateLevelCountInProject());
        _services.Add(saver);
        int levelIndex = saver.SelectedLevel;
        Level levelPrefab = Resources.LoadAll<Level>(Constants.LevelsPath)[levelIndex];
        Factory<Level> levelFactory = new(levelPrefab);
        _level = levelFactory.Create();

        if (levelIndex == 0)
        {
            Tutorial tutorialPrefab = Resources.Load<Tutorial>(Constants.TutorialPath);
            _tutorial = Instantiate(tutorialPrefab);
        }

        GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    private void RegisterServices()
    {
        _services.Add(new CarSpeedDirector());
        _services.Add(new BulletSpeedDirector());
        _services.Add(new CameraShaker());
        _services.Add(new PauseSwitcher());
        _services.Add(new CarSwipeStrategy() as ISwipeStrategy);
        _services.Add(new TypeColorRandomizer(new(_level.Colors), new(_level.Ids)));

        PoolBuilder poolBuilder = new();
        _services.Add(poolBuilder);

        _services.Add(poolBuilder.Build(_level.BulletPrefab));
        _services.Add(poolBuilder.Build(_level.SmokeParticlePrefab));
        _services.Add(poolBuilder.Build(_level.BloodParticlePrefab));
        _services.Add(poolBuilder.Build(_level.StarBangParticlePrefab));
        _services.Add(poolBuilder.Build(_level.HitParticlePrefab));

        _services.Add(new ParticleShower(_services.Get<Pool<SmokeParticle>>(), _services.Get<Pool<BloodParticle>>(), _services.Get<Pool<StarBangParticle>>(), _services.Get<Pool<HitParticle>>()));
        _services.Add(poolBuilder.Build(_level.EnemyPrefab, new EnemyConfig(_level.EnemySplineContainer, _services.Get<ParticleShower>())));
        _services.Add(new StarsCounter(_level.Stars));
        _services.Add(new SlotReservator(_level.Slots, _services.Get<ISwipeStrategy>()));
        _services.Add(new EnemySpawner(_services.Get<Pool<Enemy>>(), _services.Get<TypeColorRandomizer>(), _level.Speed));
        _services.Add(new CrowdSpawnCoordinator(this, _services.Get<EnemySpawner>(), new(_level.Crowds), _level.PortalParticle));
        _services.Add(new EnemyRegistryToAttack(_services.Get<EnemySpawner>()));
        _services.Add(new EnemiesSpeedDirector(_services.Get<EnemySpawner>(), _level.Speed));
        _services.Add(new EnemiesCounter(_services.Get<CrowdSpawnCoordinator>()));
        _services.Add(new CarMovementInitiator(_services.Get<SlotReservator>(), _services.Get<CarSpeedDirector>()));
        _services.Add(new LevelStatsHandler(_services.Get<EnemiesSpeedDirector>(), _services.Get<StarsCounter>(), _services.Get<EnemiesCounter>(), _services.Get<Saver>().SelectedLevel + 1));
    }

    private void InitializeComponents()
    {
        InitializeGuns();
        InitializeSlots();
        InitializeCars();
        InitializeStars();
        InitializeUIHandler();
        InitializeTutorial();
    }

    private void StartRun()
    {
        _services.Get<BulletSpeedDirector>().Run();
        _services.Get<EnemiesSpeedDirector>().Run();
        _services.Get<CrowdSpawnCoordinator>().Run();

        Audio.Music.PlayLevel();
    }

    private void InitializeSlots()
    {
        foreach (Rack slot in _level.Slots)
            slot.Initialize();
    }

    private void InitializeCars()
    {
        CarSpeedDirector carSpeedDirector = _services.Get<CarSpeedDirector>();
        ParticleShower particleShower = _services.Get<ParticleShower>();
        MapSplineNodes mapSplineNodes = new(_level.CarSplineContainer);
        TypeColorRandomizer colorRandomizer = _services.Get<TypeColorRandomizer>();

        foreach (Car car in _level.Cars)
            if (car != null)
                if (colorRandomizer.TryGetColor(car.Id, out Color color))
                    car.Initialize(new CarConfig(carSpeedDirector, particleShower, mapSplineNodes, color));
    }

    private void InitializeGuns()
    {
        Pool<Bullet> bulletPool = _services.Get<Pool<Bullet>>();
        EnemyRegistryToAttack enemyRegistryToAttack = _services.Get<EnemyRegistryToAttack>();
        BulletSpeedDirector bulletSpeedDirector = _services.Get<BulletSpeedDirector>();
        ParticleShower particleShower = _services.Get<ParticleShower>();

        foreach (Gun gun in _level.Guns)
        {
            Shooter shooter = new(bulletPool, enemyRegistryToAttack, bulletSpeedDirector);
            gun.Initialize(new GunConfig(shooter, particleShower));
        }
    }

    private void InitializeStars()
    {
        EnemiesSpeedDirector enemySpeedDirector = _services.Get<EnemiesSpeedDirector>();
        ParticleShower particleShower = _services.Get<ParticleShower>();
        CameraShaker cameraShaker = _services.Get<CameraShaker>();

        foreach (Star star in _level.Stars)
            star.Initialize(new StarConfig(enemySpeedDirector, particleShower, cameraShaker));
    }

    private void InitializeUIHandler()
    {
        _uiHandler.Initialize(new LevelUiConfig(
            _services.Get<PauseSwitcher>(),
            _services.Get<Saver>(),
            _services.Get<LevelStatsHandler>()));
    }

    private void InitializeTutorial()
    {
        if (_tutorial != null)
        {
            _tutorial.Initialize(new(
                _level.PortalParticle.transform, 
                _services.Get<EnemySpawner>(),
                _level.Cars[0],
                _services.Get<ISwipeStrategy>(),
                _services.Get<EnemiesSpeedDirector>(),
                _level.Slots.ToArray(),
                _level.Stars.Last()));

        }
    }
}